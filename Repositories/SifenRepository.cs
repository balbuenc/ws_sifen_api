using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapper;
using GoldenGateAPI.Data;
using GoldenGateAPI.Entities;

using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GoldenGateAPI.Repositories
{
    public class SifenRepository : ISifenRepository
    {
        private SqlConfiguration _connectionString;
        private string _sqlConnectionString;


        private readonly IConfiguration _config;



        public SifenRepository(SqlConfiguration connectionStringg, IConfiguration config)
        {
            _config = config;
            _connectionString = connectionStringg;
            _sqlConnectionString = _config.GetConnectionString("SqlConnectionString");
        }

        protected SqlConnection dbConnection()
        {
            return new SqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<Dte>> GetAllDtes()
        {

            string sql = "";
            var db = dbConnection();

            sql = @"SELECT		de.id_documento_electronico, 
								[dbo].[f_generar_CDC] (de.id_documento_electronico) cdc,
								de.tipoDocumento, 
								de.establecimiento, 
								de.codigoSeguridadAleatorio, 
								de.punto, 
								de.numero, 
								de.descripcion, 
								de.observacion, 
								de.tipoContribuyente, 
								de.fecha, 
								de.tipoEmision, 
								de.tipoTransaccion, 
								de.tipoImpuesto, 
								de.moneda, 
								de.condicionAnticipo, 
								de.condicionTipoCambio, 
								de.cambio, 
								de.id_cliente, 
								de.id_usuario, 
								de.id_certificado, 
								de.data, 
								de.id_estado, 
								TiposEmision.tipoEmisionDescripcion, 
								Estados.estado, 
								TiposImpuesto.TipoImpuestoDescripcion, 
								TiposDocumentoElectronicos.Descripcion AS TipoDocumentoElectronicoDescripcion, 
								Establecimientos.denominacion, 
								PuntosExpedicion.punto AS PuntoExpedicionDescripcion, 
								Clientes.razonSocial, 
								TiposTransaccion.tipoTransaccionDescripcion
				FROM            DocumentosElectronicos AS de 
								inner join Estados on Estados.id_estado = de.id_estado 
								left outer join Clientes ON de.id_cliente = Clientes.id_cliente
								left outer join  TiposContribuyente on TiposContribuyente.tipo = de.tipoContribuyente
								left outer join TiposEmision on TiposEmision.tipo = de.tipoEmision
								left outer join TiposDocumentoElectronicos ON de.tipoDocumento = TiposDocumentoElectronicos.tipoDocumento 
								left outer join TiposImpuesto ON de.tipoImpuesto = TiposImpuesto.tipo 
								left outer join TiposTransaccion ON de.tipoTransaccion = TiposTransaccion.tipo 
								left outer join Establecimientos ON de.establecimiento = Establecimientos.codigo 
								left outer join PuntosExpedicion ON Establecimientos.codigo = PuntosExpedicion.codigo ";


            return await db.QueryAsync<Dte>(sql, new { });
        }

        public async Task<IEnumerable<Item>> GetDteByID(int Id)
        {

            string sql = "";
            var db = dbConnection();

            sql = @"SELECT [id_item]
                  ,[id_documento_electronico]
                  ,[codigo]
                  ,[descripcion]
                  ,[observacion]
                  ,[partidaArancelaria]
                  ,[ncm]
                  ,[unidadMedida]
                  ,[cantidad]
                  ,[precioUnitario]
                  ,[cambio]
                  ,[descuento]
                  ,[anticipo]
                  ,[pais]
                  ,[tolerancia]
                  ,[toleranciaCantidad]
                  ,[toleranciaPorcentaje]
                  ,[cdcAnticipo]
                  ,[ivaTipo]
                  ,[ivaBase]
                  ,[iva]
                  ,[lote]
                  ,[vencimiento]
                  ,[numeroSerie]
                  ,[numeroPedido]
                  ,[numeroSeguimiento]
              FROM [ws_sifen].[dbo].[Items]
              where id_documento_electronico = @Id
              order by id_item asc;";


            return await db.QueryAsync<Item>(sql, new { Id = Id });
        }



        public async Task<bool> InsertDTE(Dte dte)
        {
            try
            {
                var db = dbConnection();

                var sql = @"INSERT INTO [dbo].[DocumentosElectronicos]
                                   ([tipoDocumento]
                                   ,[establecimiento]
                                   ,[codigoSeguridadAleatorio]
                                   ,[punto]
                                   ,[numero]
                                   ,[descripcion]
                                   ,[observacion]
                                   ,[tipoContribuyente]
                                   ,[fecha]
                                   ,[tipoEmision]
                                   ,[tipoTransaccion]
                                   ,[tipoImpuesto]
                                   ,[moneda]
                                   ,[condicionAnticipo]
                                   ,[condicionTipoCambio]
                                   ,[cambio]
                                   ,[id_cliente]
                                   ,[id_usuario]
                                   ,[id_certificado]
                                   ,[id_estado])
                             VALUES
                                   (@tipoDocumento, 
                                   @establecimiento,
                                   @codigoSeguridadAleatorio,
                                   @punto, 
                                   @numero,
                                   @descripcion,
                                   @observacion,
                                   @tipoContribuyente,
                                   @fecha, 
                                   @tipoEmision,
                                   @tipoTransaccion,
                                   @tipoImpuesto,
                                   @moneda,
                                   @condicionAnticipo,
                                   @condicionTipoCambio,
                                   @cambio, 
                                   @id_cliente,
                                   @id_usuario,
                                   @id_certificado,
                                   1)";

                var result = await db.ExecuteAsync(sql, new
                {
                    dte.tipoDocumento,
                    dte.establecimiento,
                    dte.codigoSeguridadAleatorio,
                    dte.punto,
                    dte.numero,
                    dte.descripcion,
                    dte.observacion,
                    dte.tipoContribuyente,
                    dte.fecha,
                    dte.tipoEmision,
                    dte.tipoTransaccion,
                    dte.tipoImpuesto,
                    dte.moneda,
                    dte.condicionAnticipo,
                    dte.condicionTipoCambio,
                    dte.cambio,
                    dte.id_cliente,
                    dte.id_usuario,
                    dte.id_certificado
                }
                );

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
