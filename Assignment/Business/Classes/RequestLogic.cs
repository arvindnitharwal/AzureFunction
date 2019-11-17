using System;
using System.Threading.Tasks;
using Assignment.Business.Common;
using Assignment.DataAccess.Interfaces;
using Assignment.Entities;
using Assignment.Business.Interfaces;
using System.Collections.Generic;
namespace Assignment.Business {
    public class RequestLogic :IRequestLogic
    {
        private readonly ICallRequest _callRequest;
        private readonly IAdmin _admin;
        RequestLogic (ICallRequest callRequest, IAdmin admin) {
            _callRequest = callRequest;
            _admin = admin;
        }
        public async Task<string> InsertRequest (CustomerRequest request) {
            bool isValidRequest = CommonLogic.ValidMobileNumber (request.MobileNumber) && CommonLogic.ValidName (request.Name) && CommonLogic.ValidLoanAmount (request.LoanAmount);
            if (!isValidRequest) {
                return "Invalid Request";
            }
            return await _callRequest.InsertRequest (request);
        }
        public async Task<string> UpdateRequest (CustomerRequest request) {
            //update the request based on mobile number
            if(!CommonLogic.ValidMobileNumber(request.MobileNumber))
            {
                return "Invalid Mobile Number";
            }
            string conditionalQuery =(CommonLogic.ValidName (request.Name) ? $"Name={request.Name}" : string.Empty);
            conditionalQuery = string.Format ("{0}{1}", conditionalQuery, CommonLogic.ValidLoanAmount (request.LoanAmount) ? $"LoanAmount={request.LoanAmount}" : string.Empty);
            if (string.IsNullOrEmpty (conditionalQuery)) {
                return "Invalid Update Request";
            }
            return await _callRequest.UpdateRequest (request, conditionalQuery);
        }
        public async Task<string> DeleteRequest (DeleteRequest request) {
            //validate the user
            bool isAdmin = await _admin.ValidUser (request.AdminEmail);
            if (!isAdmin && !CommonLogic.ValidMobileNumber(request.MobileNumber) ) {
                return "Not Allowed";
            }
            return await _callRequest.DeleteRequest (request.MobileNumber);
        }
        public async Task<List<FetchResponse>> GetUserRequest (FetchRequest request) {
            //return max 1000 record
            request.Count=request.Count>1000 ? 1000 : request.Count;
            return await _callRequest.GetFilteredresponse (request);
        }
    }
}