using Dapper;
using GoldenGateAPI.Data;
using GoldenGateAPI.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GoldenGateAPI.Repositories
{
    public class FracctionRepository : IFracctionRepository
    {
        private SqlConfiguration _connectionString;

        public FracctionRepository(SqlConfiguration connectionStringg)
        {
            _connectionString = connectionStringg;
        }

        protected NpgsqlConnection dbConnection()
        {
            return new NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteFraction(int id)
        {
            var db = dbConnection();

            var sql = @"DELETE from areas
                        WHERE id_area = @Id ";

            var result = await db.ExecuteAsync(sql, new { Id = id });

            return result > 0;
        }

        public async Task<IEnumerable<Fraccion>> GetAllFractions()
        {
            var db = dbConnection();
            var sql = @"select f.*, p.*
                        from fracciones f
                        inner
                        join asignacion a  on a.id_fraccion = f.id
                        inner
                        join propietarios p  on a.id_propietario = p.id ";

            // return await db.QueryAsync<Fraccion>(sql, new { });

            var fractionDictionary = new Dictionary<int, Fraccion>();


            return await db.QueryAsync<Fraccion, Propietario, Fraccion>(sql,
               (fraccion, propietario) =>
               {
                   Fraccion fraccionEntry;

                   if(!fractionDictionary.TryGetValue(fraccion.Id, out fraccionEntry ))
                   {
                       fraccionEntry = fraccion;
                       fraccionEntry.Propietarios = new List<Propietario>();
                       fractionDictionary.Add(fraccionEntry.Id, fraccionEntry);

                   }

                   fraccionEntry.Propietarios.Add(propietario); 
                   return fraccionEntry; 
               },
              
               splitOn: "Id");
        }

        public async Task<Fraccion> GetFractionDetails(int id)
        {
            var db = dbConnection();
            var sql = "select * from areas  where id_area = @Id";


            return await db.QueryFirstOrDefaultAsync<Fraccion>(sql, new { Id = id });
        }

        public async Task<bool> InsertFraction(Fraccion fraccion)
        {
            var db = dbConnection();

            var sql = @"INSERT INTO public.areas (area) VALUES(@area);";

            var result = await db.ExecuteAsync(sql, new
            {
                fraccion.Id
            }
            );

            return result > 0;
        }

        public async Task<bool> UpdateFraction(Fraccion fraccion)
        {
            var db = dbConnection();

            var sql = @"UPDATE public.areas
                        set area =@area
                        where id_area=@id_area;";

            var result = await db.ExecuteAsync(sql, new
            {
                fraccion.Id,
                fraccion.nombre
            }
            );

            return result > 0;
        }
    }
}
