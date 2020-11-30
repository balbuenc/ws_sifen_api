﻿using GoldenGateAPI.Entities;
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

        Task<Inmo_Fraccion> GetFractionDetails(int id);
    }
}