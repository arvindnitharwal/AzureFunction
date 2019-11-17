using System.Threading.Tasks;
using Assignment.Entities;
using System.Collections.Generic;
namespace Assignment.DataAccess.Interfaces
{
    public interface ICallRequest
    {
        Task<string> InsertRequest (CustomerRequest request);
        Task<string> UpdateRequest(CustomerRequest request,string conditionalQuery);
        Task<string> DeleteRequest(double mobileNumber);
        Task<List<FetchResponse>> GetFilteredresponse(FetchRequest request);
    }
}