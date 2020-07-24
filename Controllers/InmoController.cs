using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GoldenGateAPI.Data;
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
    [Route("inmo/[controller]")]
    [ApiController]
    public class InmoController : ControllerBase
    {
        private readonly ILogger<InmoController> _logger;
        private readonly IConfiguration _config;

        //Database ORMs
        private Oraculo OracleDB;
        private Database SqlServerDB;

        private string sqlDataSource;
        private string oracleDataSource;

        public InmoController(ILogger<InmoController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            //Get the Databases Connection Strings 
            sqlDataSource = config.GetConnectionString("DefaultConnection");
            oracleDataSource = config.GetConnectionString("OracleConnection");
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
           
            DataTable sqlDT;
            DataTable oracleDT;

            SqlServerDB = new Database(_logger, _config);
            OracleDB = new Oraculo(_logger, _config);


            _logger.LogInformation("Llamado al metodo Get(id) [HttpGet]");

            var parameters = new OracleParameter[]
               {
                    new OracleParameter("CodTransaccion", id)
               };




            //Execute ORACLE Request
            oracleDT = OracleDB.GetConsultByTransactionCode(oracleDataSource, parameters);
            var oracleResults = Tools.DataTableToJSON(oracleDT);


            //Execuete SQL Server Query
            //sqlDT = SqlServerDB.GetData("[dbo].[sp_get_personas]", sqlDataSource);
            //sqlResult = Tools.DataTableToJSON(sqlDT);



            return Ok(oracleResults);
        }

        // POST api/<InmoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
