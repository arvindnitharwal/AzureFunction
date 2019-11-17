using System.Threading.Tasks;
namespace Assignment.DataAccess.Interfaces
{
    public interface IAdmin
    {
        Task<bool> ValidUser (string email);
    }
}