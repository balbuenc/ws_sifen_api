﻿using Dapper;
using GoldenGateAPI.Data;
using GoldenGateAPI.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenGateAPI.Repositories
{
    public class OraFracctionRepository : IOraFracctionRepository
    {
        private SqlConfiguration _connectionString;

        public OraFracctionRepository(SqlConfiguration connectionStringg)
        {
            _connectionString = connectionStringg;
        }

        protected OracleConnection dbConnection()
        {
            return new OracleConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<PW_Lotes_Libres>> GetAllLotesLibres(FractionPayload payload)
        {
            string proc = "";
            string sql = "";
            var db = dbConnection();

            proc = @"CALL CONSULTAS_PAGINA_WEB.PF_LOTES_LIBRES_POR_FRACCION(:CANTIDAD)";
            await db.QueryAsync<PW_Lotes_Libres>(proc, new { CANTIDAD = payload.lotes_libres });

            sql = @"SELECT ID_FRACCION, NOMBRE, BARRIO, CIUDAD, DEPARTAMENTO, CANT_LOTES, DISPONIBLES, FECHA_APERTURA, ID_PROPIETARIO, NOMBRES, APELLIDOS, TELEFONO, EMAIL, ACTUALIZADO_AL
                    FROM INMO.PW_LOTES_LIBRES";


            return await db.QueryAsync<PW_Lotes_Libres>(sql, new { });
        }

        public async Task<Inmo_Fraccion> GetFractionDetails(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_FRACCION, NOMBRE_FRACCION, ID_CIUDAD, ID_DEPARTAMENTO, ACTUALIZADO_AL
                        FROM INMO.PW_BUSCAR_FRACCIONES";


            return await db.QueryFirstOrDefaultAsync<Inmo_Fraccion>(sql, new { Id = id });
        }
    }
}
