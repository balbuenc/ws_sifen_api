using GoldenGateAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Repositories
{
    public interface IOraFracctionRepository
    {
        Task<string> Pago(string contrato, decimal cobrado_cuota, decimal cobrado_mora, string codigo_trans);
        Task<IEnumerable<WEB_CUOTAS>> GetWebCuotasByCI(Int32 CEDULA);

        Task<IEnumerable<PW_Lotes_Libres>> GetAllLotesLibres(int lotes_libres);

        Task<IEnumerable<PW_Fracciones_Dpto>> GetAllFraccionesPorDeparatamento(string departamento);

        Task<IEnumerable<PW_Fracciones_Ciudad>> GetAllFraccionesPorCiudad(string ciudad);

        Task<IEnumerable<PW_Fracciones_Nombre>> GetAllFraccionesPorNombre(string nombre);

        Task<IEnumerable<Inmo_Fraccion>> GetFractions();
        Task<Inmo_Fraccion> GetFracctionByID(int id);

        Task<IEnumerable<Inmo_Ciudad>> GetCities();
        Task<Inmo_Ciudad> GetCityByID(int id);

        Task<IEnumerable<Inmo_Cliente>> GetClients();
        Task<Inmo_Cliente> GetClientByID(int id);

        Task<IEnumerable<Inmo_Lote>> GetLotes();
        Task<Inmo_Lote> GetLoteByID(int id);

        Task<IEnumerable<Inmo_Lote>> GetLotesByFracctionID(int id);

        Task<IEnumerable<Inmo_Lote>> GetLotesByClientID(int id);

        Task<IEnumerable<Inmo_Pago>> GetPagos();
    }
}
