using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Data
{
    public class Database
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public Database()
        { }

        public Database(ILogger logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public DataTable GetData(string str, string sqlDataSource)
        {
            DataTable objresutl = new DataTable();
            try
            {
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(str, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        objresutl.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return objresutl;
        }

        public DataTable ExecuteSP(string sp, string sqlDataSource)
        {
            DataTable objresutl = new DataTable();
            try
            {
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(sp, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myReader = myCommand.ExecuteReader();
                        objresutl.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return objresutl;
        }

        public DataTable ExecuteSP(string str, string sqlDataSource, params IDataParameter[] sqlParams)
        {
            DataTable objresutl = new DataTable();
            try
            {
                SqlDataReader myReader;
                using (SqlConnection conn = new SqlConnection(sqlDataSource))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (sqlParams != null)
                        {
                            foreach (IDataParameter para in sqlParams)
                            {
                                cmd.Parameters.Add(para);
                            }
                        }

                        myReader = cmd.ExecuteReader();
                        objresutl.Load(myReader);

                        myReader.Close();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return objresutl;
        }
    }
}
