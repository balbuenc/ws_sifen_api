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
using System.Windows.Input;

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
								de.condicionOperacion,
								de.condicionCredito,
								de.plazoCredito,

								TiposEmision.tipoEmisionDescripcion, 
								Estados.estado, 
								TiposImpuesto.TipoImpuestoDescripcion, 
								TiposDocumentoElectronicos.Descripcion AS TipoDocumentoElectronicoDescripcion, 
								Establecimientos.denominacion, 
								PuntosExpedicion.punto AS PuntoExpedicionDescripcion, 
								Clientes.razonSocial, 
								TiposTransaccion.tipoTransaccionDescripcion,
								CondicionesOperacion.CondicionDescripcion,
								CondicionesCredito.creditoDescripcion,
								Monedas.monedaDescripcion
				FROM            DocumentosElectronicos AS de 
								inner join Estados on Estados.id_estado = de.id_estado 
								left outer join Clientes ON de.id_cliente = Clientes.id_cliente
								left outer join  TiposContribuyente on TiposContribuyente.tipo = de.tipoContribuyente
								left outer join TiposEmision on TiposEmision.tipo = de.tipoEmision
								left outer join TiposDocumentoElectronicos ON de.tipoDocumento = TiposDocumentoElectronicos.tipoDocumento 
								left outer join TiposImpuesto ON de.tipoImpuesto = TiposImpuesto.tipo 
								left outer join TiposTransaccion ON de.tipoTransaccion = TiposTransaccion.tipo 
								left outer join Establecimientos ON de.establecimiento = Establecimientos.codigo 
								left outer join PuntosExpedicion ON Establecimientos.codigo = PuntosExpedicion.codigo
								left outer join CondicionesOperacion on CondicionesOperacion.condicion = de.condicionOperacion
								left outer join CondicionesCredito on CondicionesCredito.credito = de.condicionCredito
								left outer join Monedas on Monedas.moneda = de.moneda ";


            return await db.QueryAsync<Dte>(sql, new { });
        }

        public async Task<IEnumerable<Dte>> GetDteByID(Int32 Id)
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
								de.condicionOperacion,
								de.condicionCredito,
								de.plazoCredito,

								TiposEmision.tipoEmisionDescripcion, 
								Estados.estado, 
								TiposImpuesto.TipoImpuestoDescripcion, 
								TiposDocumentoElectronicos.Descripcion AS TipoDocumentoElectronicoDescripcion, 
								Establecimientos.denominacion, 
								PuntosExpedicion.punto AS PuntoExpedicionDescripcion, 
								Clientes.razonSocial, 
								TiposTransaccion.tipoTransaccionDescripcion,
								CondicionesOperacion.CondicionDescripcion,
								CondicionesCredito.creditoDescripcion,
								Monedas.monedaDescripcion
				FROM            DocumentosElectronicos AS de 
								inner join Estados on Estados.id_estado = de.id_estado 
								left outer join Clientes ON de.id_cliente = Clientes.id_cliente
								left outer join  TiposContribuyente on TiposContribuyente.tipo = de.tipoContribuyente
								left outer join TiposEmision on TiposEmision.tipo = de.tipoEmision
								left outer join TiposDocumentoElectronicos ON de.tipoDocumento = TiposDocumentoElectronicos.tipoDocumento 
								left outer join TiposImpuesto ON de.tipoImpuesto = TiposImpuesto.tipo 
								left outer join TiposTransaccion ON de.tipoTransaccion = TiposTransaccion.tipo 
								left outer join Establecimientos ON de.establecimiento = Establecimientos.codigo 
								left outer join PuntosExpedicion ON Establecimientos.codigo = PuntosExpedicion.codigo
								left outer join CondicionesOperacion on CondicionesOperacion.condicion = de.condicionOperacion
								left outer join CondicionesCredito on CondicionesCredito.credito = de.condicionCredito
								left outer join Monedas on Monedas.moneda = de.moneda
                where de.id_documento_electronico = @Id";


            return await db.QueryAsync<Dte>(sql, new { Id = Id });
        }

        public async Task<IEnumerable<Item>> GetDteItemsByID(Int32 Id)
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



        public async Task<IEnumerable<Dte>> InsertDTE(Dte dte)
        {
            try
            {
                var db = dbConnection();

                var sql = @"EXECUTE [dbo].[sp_Insert_dte] 
                                            @tipoDocumento, 
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
                                            @id_usuario,
                                            @id_certificado,
                                            @condicionOperacion,
                                            @condicionCredito,
                                            @plazoCredito,
                                            @presencia,
                                            @tipoPago,

                                            @cli_contribuyente,
                                            @cli_ruc,
                                            @cli_razonSocial,
                                            @cli_nombreFantasia,
                                            @cli_tipoOperacion,
                                            @cli_direccion,
                                            @cli_numeroCasa,
                                            @cli_departamento,
                                            @cli_distrito,
                                            @cli_ciudad,
                                            @cli_pais,
                                            @cli_TipoContribuyente,
                                            @cli_documentoTipo,
                                            @cli_documentoNumero,
                                            @cli_telefono,
                                            @cli_celular,
                                            @cli_email,
                                            @cli_codigo";

                var result = await db.QueryAsync<Dte>(sql, new
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
                    dte.id_usuario,
                    dte.id_certificado,
                    dte.condicionOperacion,
                    dte.condicionCredito,
                    dte.plazoCredito,
                    dte.presencia,
                    dte.tipoPago,

                    //cliente
                    dte.cli_contribuyente,
                    dte.cli_ruc,
                    dte.cli_razonSocial,
                    dte.cli_nombreFantasia,
                    dte.cli_tipoOperacion,
                    dte.cli_direccion,
                    dte.cli_numeroCasa,
                    dte.cli_departamento,
                    dte.cli_distrito,
                    dte.cli_ciudad,
                    dte.cli_pais,
                    dte.cli_tipoContribuyente,
                    dte.cli_documentoTipo,
                    dte.cli_documentoNumero,
                    dte.cli_telefono,
                    dte.cli_celular,
                    dte.cli_email,
                    dte.cli_codigo
                }
                );

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> InsertItem(Item item)
        {
            try
            {
                var db = dbConnection();

                var sql = @"EXECUTE [dbo].[sp_Items_insert] 
                                       @id_documento_electronico
                                      ,@codigo
                                      ,@descripcion
                                      ,@observacion
                                      ,@partidaArancelaria
                                      ,@ncm
                                      ,@unidadMedida
                                      ,@cantidad
                                      ,@precioUnitario
                                      ,@cambio
                                      ,@descuento
                                      ,@anticipo
                                      ,@pais
                                      ,@tolerancia
                                      ,@toleranciaCantidad
                                      ,@toleranciaPorcentaje
                                      ,@cdcAnticipo
                                      ,@ivaTipo
                                      ,@ivaBase
                                      ,@iva
                                      ,@lote
                                      ,@vencimiento
                                      ,@numeroSerie
                                      ,@numeroPedido
                                      ,@numeroSeguimiento";

                var result = await db.ExecuteAsync(sql, new
                {
                    item.id_documento_electronico
                    ,
                    item.codigo
                    ,
                    item.descripcion
                    ,
                    item.observacion
                    ,
                    item.partidaArancelaria
                    ,
                    item.ncm
                    ,
                    item.unidadMedida
                    ,
                    item.cantidad
                    ,
                    item.precioUnitario
                    ,
                    item.cambio
                    ,
                    item.descuento
                    ,
                    item.anticipo
                    ,
                    item.pais
                    ,
                    item.tolerancia
                    ,
                    item.toleranciaCantidad
                    ,
                    item.toleranciaPorcentaje
                    ,
                    item.cdcAnticipo
                    ,
                    item.ivaTipo
                    ,
                    item.ivaBase
                    ,
                    item.iva
                    ,
                    item.lote
                    ,
                    item.vencimiento
                    ,
                    item.numeroSerie
                    ,
                    item.numeroPedido
                    ,
                    item.numeroSeguimiento
                }
                );

                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<bool> InsertItems(List<Item> items)
        {
            try
            {
                var db = dbConnection();

                foreach (var i in items)
                {
                    var sql = @"EXECUTE [dbo].[sp_Items_insert] 
                                       @id_documento_electronico
                                      ,@codigo
                                      ,@descripcion
                                      ,@observacion
                                      ,@partidaArancelaria
                                      ,@ncm
                                      ,@unidadMedida
                                      ,@cantidad
                                      ,@precioUnitario
                                      ,@cambio
                                      ,@descuento
                                      ,@anticipo
                                      ,@pais
                                      ,@tolerancia
                                      ,@toleranciaCantidad
                                      ,@toleranciaPorcentaje
                                      ,@cdcAnticipo
                                      ,@ivaTipo
                                      ,@ivaBase
                                      ,@iva
                                      ,@lote
                                      ,@vencimiento
                                      ,@numeroSerie
                                      ,@numeroPedido
                                      ,@numeroSeguimiento";

                    var result = await db.ExecuteAsync(sql, new
                    {
                       i.id_documento_electronico,
                       i.codigo,
                       i.descripcion,
                       i.observacion,
                       i.partidaArancelaria,
                       i.ncm,
                       i.unidadMedida,
                       i.cantidad,
                       i.precioUnitario,
                       i.cambio,
                       i.descuento,
                       i.anticipo,
                       i.pais,
                       i.tolerancia,
                       i.toleranciaCantidad,
                       i.toleranciaPorcentaje,
                       i.cdcAnticipo,
                       i.ivaTipo,
                       i.ivaBase,
                       i.iva,
                       i.lote,
                       i.vencimiento,
                       i.numeroSerie,
                       i.numeroPedido,
                       i.numeroSeguimiento
                    }
                    );

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Response>> SendDTE(Command command)
        {

            try
            {
                var db = dbConnection();

                var sql = @"EXECUTE [api].[sp_call_sifen] 
                                       @id_documento_electronico
                                      ,@command
                                      ,@numero";



                return await db.QueryAsync<Response>(sql, new
                {
                    command.id_documento_electronico,
                    command.command,
                    command.numero
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Response>> SendBatchDTE(Command command)
        {

            try
            {
                var db = dbConnection();

                var sql = @"EXECUTE [api].[sp_call_sifen_batch] 
                                       @id_documento_electronico
                                      ,@command
                                      ,@numero";



                return await db.QueryAsync<Response>(sql, new
                {
                    command.id_documento_electronico,
                    command.command,
                    command.numero
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Operation>> GetOperationsByDteID(Int32 Id)
        {

            try
            {
                var db = dbConnection();

                var sql = @"select * from operaciones where id_documento_electronico = @Id order by fecha desc;";



                return await db.QueryAsync<Operation>(sql, new
                {
                    Id = Id
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
