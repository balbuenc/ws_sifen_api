using Dapper;
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

        public async Task<IEnumerable<PW_Fracciones_Dpto>> GetAllFraccionesPorDeparatamento(FractionPayload payload)
        {
            string proc = "";
            string sql = "";
            var db = dbConnection();

            proc = @"CALL CONSULTAS_PAGINA_WEB.PF_FRACCIONES_POR_DPTO(:DEPARTAMENTO)";
            await db.QueryAsync<PW_Fracciones_Dpto>(proc, new { DEPARTAMENTO = payload.departamento });

            sql = @"SELECT ID_FRACCION, NOMBRE_FRACCION, ID_DEPARTAMENTO, NOMBRE_DEPARTAMENTO, ACTUALIZADO_AL
                       FROM INMO.PW_FRACCIONES_POR_DPTO ORDER BY ID_FRACCION";


            return await db.QueryAsync<PW_Fracciones_Dpto>(sql, new { });
        }

        public async Task<IEnumerable<PW_Fracciones_Ciudad>> GetAllFraccionesPorCiudad(FractionPayload payload)
        {
            string proc = "";
            string sql = "";
            var db = dbConnection();

            proc = @"CALL CONSULTAS_PAGINA_WEB.PF_FRACCIONES_POR_CIUDAD(:CIUDAD)";
            await db.QueryAsync<PW_Fracciones_Ciudad>(proc, new { CIUDAD = payload.ciudad });

            sql = @"SELECT ID_FRACCION, NOMBRE_FRACCION, ID_CIUDAD, NOMBRE_CIUDAD, ACTUALIZADO_AL
                    FROM INMO.PW_FRACCIONES_POR_CIUDAD ORDER BY ID_FRACCION";


            return await db.QueryAsync<PW_Fracciones_Ciudad>(sql, new { });
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
