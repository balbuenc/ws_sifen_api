using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldenGateAPI.Helpers;
using GoldenGateAPI.Entities;
using GoldenGateAPI.Data;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Extensions.Logging;

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


        private List<User> _users = new List<User>();


        public UserService(IConfiguration config)
        {
            _config = config;
          

            PostgreSQL db = new PostgreSQL();

            string postgresDataSource = config.GetConnectionString("PostgresConnectionString");

            //Set User identity data query
            string query = "select Id, Username, Password, FirstName, LastName from secure.identity";
            DataTable dt = db.GetData(query, postgresDataSource);

            _users = Tools.ConvertDataTable<User>(dt);

        }



        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.username == username && x.password == password));

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
