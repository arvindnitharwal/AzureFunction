using System.Threading.Tasks;
using Assignment.Entities;
namespace Assignment.DataAccess.Interfaces
{
    public interface ICallRequest
    {
        Task<string> InsertRequest (CustomerRequest request);
        Task<string> UpdateRequest(CustomerRequest request,string conditionalQuery);
        Task<string> DeleteRequest(double mobileNumber);
    }
}