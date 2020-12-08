using GoldenGateAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Repositories
{
    public interface IOraFracctionRepository
    {
        Task<IEnumerable<PW_Lotes_Libres>> GetAllLotesLibres(FractionPayload payload);

        Task<IEnumerable<PW_Fracciones_Dpto>> GetAllFraccionesPorDeparatamento(FractionPayload payload);

        Task<IEnumerable<PW_Fracciones_Ciudad>> GetAllFraccionesPorCiudad(FractionPayload payload);

        Task<IEnumerable<PW_Fracciones_Nombre>> GetAllFraccionesPorNombre(FractionPayload payload);

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
