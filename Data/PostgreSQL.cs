using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Data
{
    public class PostgreSQL
    {

        private readonly ILogger _logger;
        private NpgsqlConnection conn;

        public PostgreSQL()
        {
         
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
            _logger = loggerFactory.CreateLogger("TEST");

        }

        //DTO for execute generic query
        public DataTable GetData(string str, string postgresDataSource)
        {
            DataTable objresutl = new DataTable();

            try
            {
                conn = new NpgsqlConnection(postgresDataSource);
                conn.Open();


                NpgsqlCommand command = new NpgsqlCommand(str, conn);
                

                var reader = command.ExecuteReader();

                objresutl.Load(reader);

                reader.Close();
                conn.Close();

                _logger.LogInformation("[{0}][PostgreSQL] GetData(..), Executed Correctly. ", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("[{0}][PostgreSQL] GetData(..), ERROR: {1}.", DateTime.Now.ToString(), ex.Message);
                return null;
            }
            return objresutl;
        }
    }
}
