using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GoldenGateAPI.Data;
using GoldenGateAPI.Entities;
using GoldenGateAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoldenGateAPI.Controllers
{
    [Authorize]
    [Route("api/cuotas")]
    [ApiController]


    public class InmoController : ControllerBase
    {
        private readonly ILogger<InmoController> _logger;
        private readonly IConfiguration _config;

        //Database ORMs
        private Oraculo OracleDB;
        private string oracleConnectionString;

        private PostgreSQL PostgresDB;
        private string postgresConnectionString;

        public InmoController(ILogger<InmoController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            //Get the Databases Connection Strings 
            postgresConnectionString = config.GetConnectionString("PostgresConnectionString");
            oracleConnectionString = config.GetConnectionString("OracleConnectionString");
        }

        // GET: api/<cuotas>
        [HttpGet]
        public ActionResult<string> GetBody([FromBody] Payload value)
        {
            return null;
        }

        // GET inmo/<InmoController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            return null;
        }

        // POST api/<InmoController>
        [HttpPost]
        public  ActionResult<string> Post([FromBody] JsonElement pay)
        {
            DataTable postgresDT;
            DataTable oracleDT;


            try
            {

                PostgresDB = new PostgreSQL();
                OracleDB = new Oraculo();

                OracleDB.connection = new OracleConnection(oracleConnectionString);
                OracleDB.connection.Open();

                PostgresDB.connection = new NpgsqlConnection(postgresConnectionString);
                PostgresDB.connection.Open();


                var p = JsonSerializer.Deserialize<Payload>(pay.GetRawText());

                var parameters = new OracleParameter[]
                   {
                        new OracleParameter("CodTransaccion", p.codigoTransaccion)
                   };


                //Execute ORACLE Request
                oracleDT = OracleDB.GetConsultByTransactionCode(OracleDB.connection, parameters);
                var oracleResult = Tools.DataTableToJSON(oracleDT);


                //Execuete SQL Server Query
                postgresDT = PostgresDB.GetData("SELECT id, username, password, firstname, lastname FROM secure.identity;", PostgresDB.connection);
                var postgresResult = Tools.DataTableToJSON(postgresDT);


                _logger.LogInformation("[HttpPost] OK");

                return Ok(oracleResult);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[HttpPost] ERROR. " + ex.Message);
                return Conflict(ex.Message);
            }
        }

        // PUT api/<InmoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InmoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
