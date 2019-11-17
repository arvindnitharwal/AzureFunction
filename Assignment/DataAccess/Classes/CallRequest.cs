using System;
using System.Data;
using System.Threading.Tasks;
using Assignment.DataAccess.Interfaces;
using Assignment.Entities;
using Dapper;
using System.Linq;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;
using Assignment.Entities.Enum;
using System.Collections.Generic;
namespace Assignment.DataAccess {
    public class CallRequest : ICallRequest {
        private readonly IConnection _connection;
        private readonly ILogger _logger;
        private readonly Dictionary<Filter, Func<FetchRequest, Task<List<FetchResponse>>>> _inputMapping;
        CallRequest (IConnection connection,ILoggerFactory logFactory) {
            _connection = connection;
            _logger=logFactory.CreateLogger<CallRequest>();
            //add call back function
            _inputMapping = new Dictionary<Filter, Func<FetchRequest, Task<List<FetchResponse>>>>();
            _inputMapping.Add(Filter.DateTime, GetDateTimeFilter);
            _inputMapping.Add(Filter.MostRecent, GetMostRecentFilter);
            _inputMapping.Add(Filter.MobileNumber, GetMobileNumberFilter);
            _inputMapping.Add(Filter.Name, GetNameFilter);
            _inputMapping.Add(Filter.LoanAmount, GetLoanAmountFilter);
        }
        public async Task<string> InsertRequest (CustomerRequest request) {
            string query = "INSERT INTO nitharwal.loanrequest (Name,MobileNumbar,LoanAmount) VALUES (@name,@mobile,@amount)";
            try {
                using (IDbConnection connection = await _connection.GetMasterConnection ()) {
                    connection.Open ();
                    int result = await connection.ExecuteAsync (query, new { name = request.Name, mobile = request.MobileNumber, amount = request.LoanAmount }, commandType : CommandType.Text);
                    if (result > 0) {
                        return "Our team will call you.";
                    }
                    return "Something went wrong";
                }
            } catch (Exception ex) when (ex is MySqlException || ex is NullReferenceException || ex is Exception) {
                _logger.LogError("Error in InsertRequest",ex);
                return "Something went wrong";
            }
        }
        public async Task<string> UpdateRequest (CustomerRequest request, string subQuery) {
            string query = "UPDATE nitharwal.loanrequest SET subQuery where MobileNumber=@number;";
            try {
                using (IDbConnection connection = await _connection.GetMasterConnection ()) {
                    connection.Open ();
                    int result = await connection.ExecuteAsync (query, new { number = request.MobileNumber }, commandType : CommandType.Text);
                    if (result > 0) {
                        return "Updated Successfully.";
                    }
                    return "You are not existing customer";
                }
            } catch (Exception ex) when (ex is MySqlException || ex is NullReferenceException || ex is Exception) {
                _logger.LogError("Error in UpdateRequest",ex);
                return "Something went wrong";
            }
        }
        public async Task<string> DeleteRequest (double mobileNumber) {
            string query = "Delete from nitharwal.loanrequest where MobileNumbar = @mobileNumber";
            try {
                using (IDbConnection connection = await _connection.GetMasterConnection ()) {
                    connection.Open ();
                    int result = await connection.ExecuteAsync (query, new { mobileNumber = mobileNumber }, commandType : CommandType.Text);
                    if (result > 0) {
                        return "Deleted.";
                    }
                    return "Something went wrong";
                }
            } catch (Exception ex) when (ex is MySqlException || ex is NullReferenceException || ex is Exception) {
                _logger.LogError("Error in DeleteRequest",ex);
                return "Something went wrong";
            }
        }
        public async Task<List<FetchResponse>> GetFilteredresponse(FetchRequest request)
        {
            return await _inputMapping[request.Filter](request);
        }
        public async Task<List<FetchResponse>> GetDateTimeFilter(FetchRequest request)
        {
            string query="Select Name,LoanAmount from nitharwal.loanrequest where EntryDate > @entrydate orderby EntryDate desc limit"+request.Count;
            return await GetUserDetails(request,query);
        }
        public async Task<List<FetchResponse>> GetLoanAmountFilter(FetchRequest request)
        {
            string query="Select Name,LoanAmount from nitharwal.loanrequest where LoanAmount > @loanamount orderby EntryDate desc limit"+request.Count;
            return await GetUserDetails(request,query);
        }
        public async Task<List<FetchResponse>> GetMobileNumberFilter(FetchRequest request)
        {
            string query="Select Name,LoanAmount from nitharwal.loanrequest where MobileNumber > @mobilenumber orderby EntryDate desc limit"+request.Count;
            return await GetUserDetails(request,query);
        }
        public async Task<List<FetchResponse>> GetMostRecentFilter(FetchRequest request)
        {
            string query="Select Name,LoanAmount from nitharwal.loanrequest orderby EntryDate desc limit"+request.Count;
            return await GetUserDetails(request,query);
        }
        public async Task<List<FetchResponse>> GetNameFilter(FetchRequest request)
        {
            string query="Select Name,LoanAmount from nitharwal.loanrequest where Name = @name";
            return await GetUserDetails(request,query);
        }
        private async Task<List<FetchResponse>> GetUserDetails(FetchRequest request,string query)
        {
            try {
                using (IDbConnection connection = await _connection.GetMasterConnection ()) {
                    connection.Open ();
                    return (await connection.QueryAsync<FetchResponse>(query, new { mobilenumber = request.MobileNumber,entrydate=request.EntryDate,name=request.Name }, commandType : CommandType.Text))?.ToList()?? new List<FetchResponse>();
                }
            } catch (Exception ex) when (ex is MySqlException || ex is NullReferenceException || ex is Exception) {
                _logger.LogError("Error in DeleteRequest",ex);
                return new List<FetchResponse>();
            }
        }
    }
}