using Assignment.Entities;
using System.Threading.Tasks;
namespace Assignment.Business.Interfaces
{
    public interface IRequestLogic
    {
        Task<string> InsertRequest (CustomerRequest request);
        Task<string> UpdateRequest (CustomerRequest request);
        Task<string> DeleteRequest (DeleteRequest request);
    }
}