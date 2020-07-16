using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GoldenGateAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GoldenGateAPI.Controllers
{

    

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        Database db = new Database();

        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ILogger<UsuariosController> logger)
        {
            _logger = logger;
        }

        // GET: api/<UsuariosController>
        [HttpGet]
        public ActionResult Get()
        {

            _logger.LogInformation("Get Petition");
            //return new string[] { "value1", "value2" };
            string query = "SELECT  [Id],[UserName], [PasswordHash]  FROM [GoldengateDB].[dbo].[AspNetUsers]";
            DataTable dt = db.GetData(query);
            var result = new ObjectResult(dt);
            //return result;

            return Ok("value");
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
