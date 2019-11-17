using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Assignment.DataAccess.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;
namespace Assignment.DataAccess {
    public class Admin : IAdmin {
        private readonly IConnection _connection;
         private readonly ILogger _logger;
        Admin (IConnection connection,ILoggerFactory logFactory) {
            _connection = connection;
            _logger=logFactory.CreateLogger<Admin>();
        }
        public async Task<bool> ValidUser (string email) {
            string query = "Select Count(Id) where Email=@email";
            try {
                using (IDbConnection connection = await _connection.GetMasterConnection ()) {
                    connection.Open ();
                    int result = (await connection.QueryAsync<int> (query, commandType : CommandType.Text)).FirstOrDefault ();
                    if (result > 0) {
                        return true;
                    }
                    return false;
                }
            } catch (Exception ex) when (ex is MySqlException || ex is NullReferenceException || ex is Exception) {
                 _logger.LogError("Error in ValidUser",ex);
                return false;
            }
        }
    }
}