using System.Data;
using System.Threading.Tasks;
namespace Assignment.DataAccess.Interfaces{
    public interface IConnection
    {
        Task<IDbConnection> GetMasterConnection();
    }
}