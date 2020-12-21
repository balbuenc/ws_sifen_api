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

        [HttpPost("api/pago")]
        public async Task<IActionResult> Pago([FromQuery] string contrato, [FromQuery] decimal cobrado_cuota, [FromQuery] decimal cobrado_mora, [FromQuery] string codigo_trans)
        {

            _logger.LogInformation("[{1}][HttpGet] Pago()", DateTime.Now.ToString());


            return Ok(await _OraFracctionRepository.Pago(contrato, cobrado_cuota, cobrado_mora, codigo_trans));
        }

        [HttpGet("api/cuotas/{CEDULA}")]
        public async Task<IActionResult> GetWebCuotasByCI( Int32 CEDULA)
        {

            _logger.LogInformation("[{1}][HttpGet] GetWebCuotasByCI()", DateTime.Now.ToString());


            return Ok(await _OraFracctionRepository.GetWebCuotasByCI( CEDULA));
        }

        [HttpGet("api/fracciones")]
        public async Task<IActionResult> GetAllFractions()
        {

            _logger.LogInformation("[{1}][HttpGet] GetAllFractions()", DateTime.Now.ToString());
           

            return Ok(await _OraFracctionRepository.GetFractions());
        }

        [HttpGet("api/fracciones/{id}")]
        public async Task<IActionResult> GetFractionByID(int id)
        {
            _logger.LogInformation("[{1}][HttpGet] GetFractionByID()", DateTime.Now.ToString());
            return Ok(await _OraFracctionRepository.GetFracctionByID(id));
        }

        [HttpGet("api/fracciones/{id}/lotes")]
        public async Task<IActionResult> GetLotesByFractionID(int id)
        {
            _logger.LogInformation("[{1}][HttpGet] GetLotesByFractionID()", DateTime.Now.ToString());
            return Ok(await _OraFracctionRepository.GetLotesByFracctionID(id));
        }

        [HttpGet("api/ciudades")]
        public async Task<IActionResult> GetAllCities()
        {

            _logger.LogInformation("[{1}][HttpGet] GetAllCities()", DateTime.Now.ToString());
           
            return Ok(await _OraFracctionRepository.GetCities());
        }

        [HttpGet("api/ciudades/{id}")]
        public async Task<IActionResult> GetCityByID(int id)
        {

            _logger.LogInformation("[{1}][HttpGet] GetCityByID()", DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetCityByID(id));
        }



        [HttpGet("api/clientes")]
        public async Task<IActionResult> GetAllClients()
        {

            _logger.LogInformation("[{1}][HttpGet] GetAllClients()", DateTime.Now.ToString());
            
            return Ok(await _OraFracctionRepository.GetClients());
        }

        [HttpGet("api/clientes/{id}")]
        public async Task<IActionResult> GetClientByID(int id)
        {

            _logger.LogInformation("[{1}][HttpGet] GetClientByID()", DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetClientByID(id));
        }

        [HttpGet("api/clientes/{id}/lotes")]
        public async Task<IActionResult> GetLotesByClientID(int id)
        {

            _logger.LogInformation("[{1}][HttpGet] GetLotesByClientID()", DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetLotesByClientID(id));
        }

        [HttpGet("api/lotes")]
        public async Task<IActionResult> GetAllLotes()
        {

            _logger.LogInformation("[{1}][HttpGet] GetAllLotes()", DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetLotes());
        }

        [HttpGet("api/lotes/{id}")]
        public async Task<IActionResult> GetLoteByID(int id)
        {

            _logger.LogInformation("[{1}][HttpGet] GetLoteByID()", DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetLoteByID(id));
        }

        [HttpGet("api/pagos")]
        public async Task<IActionResult> GetPagos()
        {

            _logger.LogInformation("[{1}][HttpGet] GetPagos()", DateTime.Now.ToString());

            return Ok(await _OraFracctionRepository.GetPagos());
        }


        [HttpGet("/api/fracciones/buscar")]
        public async Task<IActionResult> GetFractions([FromQuery] int lotes_libres, [FromQuery]  string departamento, [FromQuery] string ciudad, [FromQuery] string nombre)
        {
            _logger.LogInformation("[{1}][HttpGet] GetFractions([FromQuery] int lotes_libres)", DateTime.Now.ToString());


            if (lotes_libres > 0)
            {
                return Ok(await _OraFracctionRepository.GetAllLotesLibres(lotes_libres));
            }
            else if(departamento != "" && departamento != null)
            {
                return Ok(await _OraFracctionRepository.GetAllFraccionesPorDeparatamento(departamento));
            }
            else if (ciudad != "" && ciudad != null)
            {
                return Ok(await _OraFracctionRepository.GetAllFraccionesPorCiudad(ciudad));
            }
            else if (nombre != "" && nombre != null)
            {
                return Ok(await _OraFracctionRepository.GetAllFraccionesPorNombre(nombre));
            }
            else
            {
                return NotFound();
            }
        }

      
    }
}
