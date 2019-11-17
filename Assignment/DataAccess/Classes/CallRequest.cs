using System;
using System.Data;
using System.Threading.Tasks;
using Assignment.DataAccess.Interfaces;
using Assignment.Entities;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;
namespace Assignment.DataAccess {
    public class CallRequest : ICallRequest {
        private readonly IConnection _connection;
        private readonly ILogger _logger;
        CallRequest (IConnection connection,ILoggerFactory logFactory) {
            _connection = connection;
            _logger=logFactory.CreateLogger<CallRequest>();
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
            string query = "Delete from notifications.TempPayload where MobileNumbar = @mobileNumber";
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
    }
}