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
 
        public NpgsqlConnection connection;

        public PostgreSQL()
        {
        }

        //DTO for execute generic query
        public DataTable GetData(string query, NpgsqlConnection conn)
        {
            DataTable objresutl = new DataTable();

            try
            {
                NpgsqlCommand command = new NpgsqlCommand(query, conn);
                var reader = command.ExecuteReader();

                objresutl.Load(reader);

                reader.Close();
              
            }
            catch (Exception ex)
            {
                return null;
            }
            return objresutl;
        }

        ~PostgreSQL()
        {
            connection.Close();
        }
    }
}
