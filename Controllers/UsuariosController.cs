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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoldenGateAPI.Controllers
{



    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly IConfiguration _config;

       

        private Database db = new Database();
        private string sqlDataSource;

        public UsuariosController(ILogger<UsuariosController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            sqlDataSource = config.GetConnectionString("DefaultConnection");
        }



        // GET: api/<UsuariosController>
        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogInformation("Llamado al metodo Get() [HttpGet]");
            string query = "[dbo].[sp_get_personas]";

            DataTable dt = db.GetData(query, sqlDataSource);
            var result = Tools.DataTableToJSON(dt);

            return Ok(result);
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        public ActionResult GetusuarioByID(int id)
        {
            _logger.LogInformation("Llamado al metodo GetusuarioByID(int id) [HttpGet]");
            string query = "[dbo].[sp_get_persona_by_id]";

           
           

            
            var parameters = new IDataParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = db.ExecuteSP(query, sqlDataSource, parameters);
            if ( dt != null)
            {
                var result = Tools.DataTableToJSON(dt);
                return Ok(result);
            }
            else
            {
                return NotFound(new { Result = "something went wrong" });

            }
        }

        // POST api/<UsuariosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
