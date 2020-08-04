using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GoldenGateAPI.Data;
using GoldenGateAPI.Entities;
using GoldenGateAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        private PostgreSQL PostgreDB;
        private string postgresConnectionString;

        public InmoController(ILogger<InmoController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            //Get the Databases Connection Strings 
            postgresConnectionString = config.GetConnectionString("PostgresConnectionString");
            oracleConnectionString = config.GetConnectionString("OracleConnectionString");
        }

        // GET: api/<InmoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET inmo/<InmoController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
          
            DataTable postgresDT;

            PostgreDB = new PostgreSQL();
            OracleDB = new Oraculo();


            _logger.LogInformation("Llamado al metodo Get(id) [HttpGet]");

            //var parameters = new OracleParameter[]
            //   {
            //        new OracleParameter("CodTransaccion", id)
            //   };




            //Execute ORACLE Request
            //oracleDT = OracleDB.GetConsultByTransactionCode(oracleConnectionString, parameters);
            //var oracleResults = Tools.DataTableToJSON(oracleDT);


            //Execuete SQL Server Query
            postgresDT = PostgreDB.GetData("select * from users", postgresConnectionString);
            var sqlResult = Tools.DataTableToJSON(postgresDT);



            return Ok(sqlResult);
        }

        // POST api/<InmoController>
        [HttpPost]
        public async Task<ActionResult<User>> Post(User usuario)
        {
            _logger.LogInformation("Llamado al metodo POST() :" + usuario.username);
            return Ok("HTTP POST EXECUTED");
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
