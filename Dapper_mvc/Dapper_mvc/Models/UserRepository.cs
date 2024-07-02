using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dapper_mvc.Models
{
    public class UserRepository
    {

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["EquitecConnectionString"].ConnectionString;
        }

        public IEnumerable<User> GetAllUsers()
        {
            using (var db = new SqlConnection(GetConnectionString()))
            {
                return db.Query<User>("dbo.GetAllUsers", commandType: CommandType.StoredProcedure);
            }
        }

        public User GetUserById(int userId)
        {
            using (var db = new SqlConnection(GetConnectionString()))
            {
                var parameters = new { UserId = userId };
                return db.Query<User>("dbo.GetUserById", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public void CreateUser(User user)
        {
            using (var db = new SqlConnection(GetConnectionString()))
            {
                var parameters = new
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Department = user.Department,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    DOB = user.DOB
                };
                db.Execute("dbo.CreateUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateUser(User user)
        {
            using (var db = new SqlConnection(GetConnectionString()))
            {
                var parameters = new
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Department = user.Department,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    DOB = user.DOB
                };
                db.Execute("dbo.UpdateUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void SoftDeleteUser(int userId)
        {
            using (var db = new SqlConnection(GetConnectionString()))
            {
                var parameters = new { UserId = userId };
                db.Execute("dbo.SoftDeleteUser", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<User> GetDeletedUsers()
        {
            using (var db = new SqlConnection(GetConnectionString()))
            {
                return db.Query<User>("dbo.GetDeletedUsers", commandType: CommandType.StoredProcedure);
            }
        }
    }
}