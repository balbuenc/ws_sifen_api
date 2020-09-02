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
using Npgsql;

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

        public readonly bool IsConnected = false;

        public UserService(IConfiguration config)
        {
            _config = config;

            string postgresConnectionString = config.GetConnectionString("PostgresConnectionString");
            string query = "select Id, Username, Password, FirstName, LastName from secure.identity";

            try
            {


                PostgreSQL identityDB = new PostgreSQL();

                identityDB.connection = new NpgsqlConnection(postgresConnectionString);
                identityDB.connection.Open();


                //Set User identity data query

                DataTable dt = identityDB.GetData(query, identityDB.connection);

                _users = Tools.ConvertDataTable<User>(dt);

                identityDB.connection.Close();

            }
            catch (Exception ex)
            {
                throw  new Exception("Cannot authenticate user petition: " + ex.Message );
            }

        }



        public async Task<User> Authenticate(string username, string password)
        {
            if (_users == null)
                return null;

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
