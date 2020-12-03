using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GoldenGateAPI.Data;
using GoldenGateAPI.Entities;
using GoldenGateAPI.Helpers;
using GoldenGateAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace GoldenGateAPI.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [ApiController]
    public class FraccionesController : ControllerBase
    {
        private readonly ILogger<FraccionesController> _logger;
        private readonly IConfiguration _config;

        //Database ORMs
     
     
        private readonly IOraFracctionRepository _OraFracctionRepository;

        public FraccionesController(IOraFracctionRepository fractionRepository, ILogger<FraccionesController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _OraFracctionRepository = fractionRepository;
        }


        [HttpGet("api/ObtenerFracciones")]
        public async Task<IActionResult> GetAllFractions()
        {
            
            _logger.LogInformation("[HttpGet] GetFractions()");
            _logger.LogInformation("[Datetime] " + DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetAllFracciones());
        }

        [HttpGet("api/ObtenerCiudades")]
        public async Task<IActionResult> GetAllCities()
        {
            
            _logger.LogInformation("[HttpGet] GetAllCities()");
            _logger.LogInformation("[Datetime] " + DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetAllCiudades());
        }

        [HttpGet("api/fracciones")]
        public async Task<IActionResult> GetFractions([FromBody] JsonElement pay)
        {
            var p = JsonSerializer.Deserialize<FractionPayload>(pay.GetRawText());

            
            _logger.LogInformation("[HttpGet] GetFractions([FromBody] JsonElement pay)");
            _logger.LogInformation("[Datetime] " + DateTime.Now.ToString());

            if (p.lotes_libres > 0)
            {
                return Ok(await _OraFracctionRepository.GetAllLotesLibres(p));
            }
            else if (p.departamento != "" )
            {
                return Ok(await _OraFracctionRepository.GetAllFraccionesPorDeparatamento(p));
            }
            else if (p.ciudad != "" )
            {
                return Ok(await _OraFracctionRepository.GetAllFraccionesPorCiudad(p));
            }
            else if (p.nombre != "")
            {
                return Ok(await _OraFracctionRepository.GetAllFraccionesPorNombre(p));
            }

            return NotFound();
                

        }
    }
}
