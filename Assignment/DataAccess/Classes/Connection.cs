using System.Data;
using Assignment.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace Assignment.DataAccess {
    //read from appsetting.json or key value pair system
    public class Connection :IConnection
    {
        private static readonly string ConnectionString="";
        public async Task<IDbConnection> GetMasterConnection()
        {
            return new SqlConnection (ConnectionString);
        }
    }
}