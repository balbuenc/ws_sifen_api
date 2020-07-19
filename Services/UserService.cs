using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenGateAPI.Helpers;
using GoldenGateAPI.Entities;
using GoldenGateAPI.Data;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace GoldenGateAPI.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private Database db = new Database();
        private string sqlDataSource;
        private List<User> _users = new List<User>();

        public UserService(IConfiguration config)
        {
            _config = config;
            sqlDataSource = config.GetConnectionString("DefaultConnection");

            //Set User identity data query
            string query = "secure.sp_get_users";
            DataTable dt = db.ExecuteSP(query, sqlDataSource);
          
            _users = Tools.ConvertDataTable<User>(dt);
           
        }

      

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => _users.WithoutPasswords());
        }
    }
}
