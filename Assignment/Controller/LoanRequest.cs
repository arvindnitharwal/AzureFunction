using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Assignment.Business.Interfaces;
using Assignment.Entities;
using System.Collections.Generic;

namespace Assignment.Controller {
    public class LoanRequest {
         private const string route = "assignment";
         private readonly IRequestLogic _requestLogic;
         LoanRequest(IRequestLogic requestLogic)
         {
             _requestLogic=requestLogic;
         }
         //insert the customer request
        [FunctionName ("loanrequest")]
        public async Task<IActionResult> InsertRequest (
            [HttpTrigger (AuthorizationLevel.Function, "post", Route = route)] HttpRequestMessage req) 
        {
            var request=JsonConvert.DeserializeObject<CustomerRequest>(await req.Content.ReadAsStringAsync());
            string response=await _requestLogic.InsertRequest(request);
            return new OkObjectResult(response);
        }
        //update the customer request
        [FunctionName ("updaterequest")]
        public async Task<IActionResult> UpdateRequest (
            [HttpTrigger (AuthorizationLevel.Function, "put", Route = route)] HttpRequestMessage req) 
        {
            var request=JsonConvert.DeserializeObject<CustomerRequest>(await req.Content.ReadAsStringAsync());
            string response=await _requestLogic.UpdateRequest(request);
            return new OkObjectResult(response);
        }
        [FunctionName ("deleterequest")]
        public async Task<IActionResult> DeleteRequest (
            [HttpTrigger (AuthorizationLevel.Function, "delete", Route = route)] HttpRequestMessage req) 
        {
            var request=JsonConvert.DeserializeObject<DeleteRequest>(await req.Content.ReadAsStringAsync());
            string response=await _requestLogic.DeleteRequest(request);
            return new OkObjectResult(response);
        }
        //fetch user based on filters
        [FunctionName ("getuser")]
        public async Task<IActionResult> GetUserRequest (
            [HttpTrigger (AuthorizationLevel.Function, "get", Route = route)] HttpRequestMessage req) 
        {
            var request=JsonConvert.DeserializeObject<FetchRequest>(await req.Content.ReadAsStringAsync());
            List<FetchResponse> response=await _requestLogic.GetUserRequest(request);
            return new OkObjectResult(response);
        }
    }
}