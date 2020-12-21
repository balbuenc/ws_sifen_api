using Dapper;
using GoldenGateAPI.Data;
using GoldenGateAPI.Entities;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Oracle;

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

        public async Task<IEnumerable<WEB_PAGOS>> Pago(string contrato, decimal cobrado_cuota, decimal cobrado_mora, string codigo_trans)
        {
            string proc = "";
            string sql = "";


            var db = dbConnection();



            OracleDynamicParameters _params = new OracleDynamicParameters();

            _params.Add(name: "p_contrato", value: contrato, direction: ParameterDirection.Input, dbType: OracleMappingType.Varchar2);
            _params.Add(name: "p_cobrado_cuota", value: cobrado_cuota, direction: ParameterDirection.Input, dbType: OracleMappingType.Decimal);
            _params.Add(name: "p_cobrado_mora", value: cobrado_mora, direction: ParameterDirection.Input, dbType: OracleMappingType.Decimal);
            _params.Add(name: "p_codigo_trans", value: codigo_trans, direction: ParameterDirection.Input, dbType: OracleMappingType.Varchar2);
            //_params.Add(name: "p_respuesta", direction: ParameterDirection.Output, dbType: OracleMappingType.RefCursor);

            //proc = @"CALL web_cobranza.pf_pago( p_contrato => :p_contrato,
            //                                         p_cobrado_cuota => :p_cobrado_cuota,
            //                                         p_cobrado_mora => :p_cobrado_mora,
            //                                         p_codigo_trans => :p_codigo_trans,
            //                                         p_respuesta => :p_respuesta )";

            proc = @"CALL web_cobranza.pf_pago( p_contrato => :p_contrato,
                                                 p_cobrado_cuota => :p_cobrado_cuota,
                                                 p_cobrado_mora => :p_cobrado_mora,
                                                 p_codigo_trans => :p_codigo_trans)";

            //Ejecuto el pago
            await db.QueryAsync<string>(proc, _params);


            //Consulto el pago realizado
            sql = @"SELECT NUMERO_CONTRATO, ID_FRACCION, ID_MANZANA, ID_LOTE, ESTADO, FECHA_PROCESO, MONTO_COBRADO_CUOTA, MONTO_COBRADO_MORA, CODIGO_RETORNO, DESC_RETORNO, CODIGO_TRANS, PAGO_GENERADO, FECHA_GENERADO, COMPROBANTE, CUOTA_COBRADA, IVA_CUOTA, MORA_COBRADA, IVA_MORA
                    FROM INMO.WEB_PAGOS
                    WHERE CODIGO_TRANS =:CODIGO_TRANS";


            return await db.QueryAsync<WEB_PAGOS>(sql, new { CODIGO_TRANS = codigo_trans });


        }

        public async Task<IEnumerable<WEB_CUOTAS>> GetWebCuotasByCI(Int32 CEDULA)
        {
            string proc = "";
            string sql_codigo_trans = "";
            string sql = "";

            IEnumerable<WEB_CUOTAS> web_cuotas;
            Int32 ultimo_codigo_trans = 0;

            var db = dbConnection();

            sql_codigo_trans = @"SELECT * FROM (
                                SELECT  NUMERO_CONTRATO, NOMBRE_CLIENTE, ID_FRACCION, ID_MANZANA, ID_LOTE, NUMERO_CUOTA, MONTO_CUOTA, MESES_MORA, MONTO_MORA, CODIGO_RETORNO, DESC_RETORNO, CODIGO_TRANS, DOCUMENTO, FECHA_VENCIMIENTO, DESCRIPCION_FRACCION, FECHA_PROCESO
                                FROM INMO.WEB_CUOTAS
                                ORDER BY CAST (CODIGO_TRANS AS int)  desc 
                                )
                                WHERE ROWNUM <= 1";


            web_cuotas = await db.QueryAsync<WEB_CUOTAS>(sql_codigo_trans, new { });

            ultimo_codigo_trans = web_cuotas.First<WEB_CUOTAS>().CODIGO_TRANS + 1;

            proc = @"CALL web_consulta.pf_consulta(wcedula => :wcedula, wcodigo_trans => :wcodigo_trans)";

            await db.QueryAsync<WEB_CUOTAS>(proc, new { wcedula = CEDULA, wcodigo_trans = ultimo_codigo_trans });

            sql = @"SELECT NUMERO_CONTRATO, NOMBRE_CLIENTE, ID_FRACCION, ID_MANZANA, ID_LOTE, NUMERO_CUOTA, MONTO_CUOTA, MESES_MORA, MONTO_MORA, CODIGO_RETORNO, DESC_RETORNO, CODIGO_TRANS, DOCUMENTO, FECHA_VENCIMIENTO, DESCRIPCION_FRACCION, FECHA_PROCESO
                    FROM INMO.WEB_CUOTAS
                    WHERE CODIGO_TRANS = :CODIGO_TRANS";


            return await db.QueryAsync<WEB_CUOTAS>(sql, new { CODIGO_TRANS = ultimo_codigo_trans });
        }


        public async Task<IEnumerable<PW_Lotes_Libres>> GetAllLotesLibres(int lotes_libres)
        {
            string proc = "";
            string sql = "";
            var db = dbConnection();

            proc = @"CALL CONSULTAS_PAGINA_WEB.PF_LOTES_LIBRES_POR_FRACCION(:CANTIDAD)";
            await db.QueryAsync<PW_Lotes_Libres>(proc, new { CANTIDAD = lotes_libres });

            sql = @"SELECT ID_FRACCION, NOMBRE, BARRIO, CIUDAD, DEPARTAMENTO, CANT_LOTES, DISPONIBLES, FECHA_APERTURA, ID_PROPIETARIO, NOMBRES, APELLIDOS, TELEFONO, EMAIL, ACTUALIZADO_AL
                    FROM INMO.PW_LOTES_LIBRES";


            return await db.QueryAsync<PW_Lotes_Libres>(sql, new { });
        }

        public async Task<IEnumerable<PW_Fracciones_Dpto>> GetAllFraccionesPorDeparatamento(string departamento)
        {
            string proc = "";
            string sql = "";
            var db = dbConnection();

            proc = @"CALL CONSULTAS_PAGINA_WEB.PF_FRACCIONES_POR_DPTO(UPPER(:DEPARTAMENTO))";
            await db.QueryAsync<PW_Fracciones_Dpto>(proc, new { DEPARTAMENTO = departamento });

            sql = @"SELECT ID_FRACCION, NOMBRE_FRACCION, ID_DEPARTAMENTO, NOMBRE_DEPARTAMENTO, ACTUALIZADO_AL
                       FROM INMO.PW_FRACCIONES_POR_DPTO ORDER BY ID_FRACCION";


            return await db.QueryAsync<PW_Fracciones_Dpto>(sql, new { });
        }

        public async Task<IEnumerable<PW_Fracciones_Ciudad>> GetAllFraccionesPorCiudad(string ciudad)
        {
            string proc = "";
            string sql = "";
            var db = dbConnection();

            proc = @"CALL CONSULTAS_PAGINA_WEB.PF_FRACCIONES_POR_CIUDAD(:CIUDAD)";
            await db.QueryAsync<PW_Fracciones_Ciudad>(proc, new { CIUDAD = ciudad });

            sql = @"SELECT ID_FRACCION, NOMBRE_FRACCION, ID_CIUDAD, NOMBRE_CIUDAD, ACTUALIZADO_AL
                    FROM INMO.PW_FRACCIONES_POR_CIUDAD ORDER BY ID_FRACCION";


            return await db.QueryAsync<PW_Fracciones_Ciudad>(sql, new { });
        }

        public async Task<IEnumerable<PW_Fracciones_Nombre>> GetAllFraccionesPorNombre(string nombre)
        {
            string proc = "";
            string sql = "";
            var db = dbConnection();

            proc = @"CALL CONSULTAS_PAGINA_WEB.PF_FRACCIONES_POR_NOMBRE(:NOMBRE)";
            await db.QueryAsync<PW_Fracciones_Nombre>(proc, new { NOMBRE = nombre });

            sql = @"SELECT ID_FRACCION, NOMBRE_FRACCION, ID_CIUDAD, ID_DEPARTAMENTO, ACTUALIZADO_AL, NOMBRE_CIUDAD, NOMBRE_DEPARTAMENTO
                    FROM INMO.PW_FRACCIONES_POR_NOMBRE";


            return await db.QueryAsync<PW_Fracciones_Nombre>(sql, new { });
        }

        public async Task<IEnumerable<Inmo_Fraccion>> GetFractions()
        {
            var db = dbConnection();
            var sql = @"SELECT ID_FRACCION,DESCRIPCION as NOMBRE_FRACCION, CANT_MANZANA,  DIMENSION, ESTADO, FECHA_BAJA, FECHA_INGRESO, ID_CIUDAD,  NRO_FINCA, OBSERVACION, TIPO_FRACCION,  ID_DEPARTAMENTO
                        FROM INMO.FRACCIONES";


            return await db.QueryAsync<Inmo_Fraccion>(sql, new { });
        }

        public async Task<Inmo_Fraccion> GetFracctionByID(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_FRACCION,DESCRIPCION as NOMBRE_FRACCION, CANT_MANZANA,  DIMENSION, ESTADO, FECHA_BAJA, FECHA_INGRESO, ID_CIUDAD,  NRO_FINCA, OBSERVACION, TIPO_FRACCION,  ID_DEPARTAMENTO
                        FROM INMO.FRACCIONES
                        WHERE ID_FRACCION = :id";


            return await db.QueryFirstOrDefaultAsync<Inmo_Fraccion>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Inmo_Ciudad>> GetCities()
        {
            var db = dbConnection();
            var sql = @"SELECT ID_CIUDAD, DESCRIPCION AS CIUDAD
                        FROM INMO.CIUDADES order BY 2";


            return await db.QueryAsync<Inmo_Ciudad>(sql, new { });
        }

        public async Task<Inmo_Ciudad> GetCityByID(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_CIUDAD, DESCRIPCION AS CIUDAD
                        FROM INMO.CIUDADES
                        WHERE ID_CIUDAD = :id";

            return await db.QueryFirstOrDefaultAsync<Inmo_Ciudad>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Inmo_Cliente>> GetClients()
        {
            var db = dbConnection();
            var sql = @"SELECT ID_PERSONA, DIRECCION_EMAIL, DIRECCION_PARTICULAR, DOCUMENTO, ESTADO, ESTADO_CIVIL, FECHA_BAJA, FECHA_INGRESO, FECHA_NACIMIENTO, NACIONALIDAD, NOMBRE_COMPLETO, OBSERVACION, PRIMER_APELLIDO, PRIMER_NOMBRE, RUC, SEGUNDO_APELLIDO, SEGUNDO_NOMBRE, SEXO, TELEFONO_OFICINA, TELEFONO_PARTICULAR, TIPO_DOCUMENTO,  REFERENCIA_DIRECCION, BARRIO, INFORMCONF
                        FROM INMO.PERSONAS
                        WHERE PERSONA_CLIENTE = 'S'";


            return await db.QueryAsync<Inmo_Cliente>(sql, new { });
        }


        public async Task<Inmo_Cliente> GetClientByID(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT ID_PERSONA, DIRECCION_EMAIL, DIRECCION_PARTICULAR, DOCUMENTO, ESTADO, ESTADO_CIVIL, FECHA_BAJA, FECHA_INGRESO, FECHA_NACIMIENTO, NACIONALIDAD, NOMBRE_COMPLETO, OBSERVACION, PRIMER_APELLIDO, PRIMER_NOMBRE, RUC, SEGUNDO_APELLIDO, SEGUNDO_NOMBRE, SEXO, TELEFONO_OFICINA, TELEFONO_PARTICULAR, TIPO_DOCUMENTO,  REFERENCIA_DIRECCION, BARRIO, INFORMCONF
                        FROM INMO.PERSONAS
                        WHERE PERSONA_CLIENTE = 'S'
                        AND ID_PERSONA = :id";

            return await db.QueryFirstOrDefaultAsync<Inmo_Cliente>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Inmo_Lote>> GetLotes()
        {
            var db = dbConnection();
            var sql = @"SELECT CANT_DIAS_RESERVA, CODIGO_RESERVA, COSTO_LOTE, CTA_CTE_CTRAL, DIMENSION, DIMENSION_ESTE, DIMENSION_NORTE, DIMENSION_OESTE, DIMENSION_SUR, ESTADO, FECHA_CHEQUERA, FECHA_ESTADO, FECHA_INGRESO, ID_COMISION, ID_FRACCION, ID_LOTE, ID_MANZANA, LINDERO_ESTE, LINDERO_NORTE, LINDERO_OESTE, LINDERO_SUR, MONTO_CUOTA, NRO_FINCA, NUMERO_CONTRATO, OBSERVACION, PLAZO, PRECIO_CONTADO, PRECIO_FINANCIADO, ID_CLIENTE, ID_VENDEDOR, PRECIO_VENTA_FINAL, TOTAL_PAGADO, SALDO_DEUDOR, ULTIMA_CUOTA_PAGADA, FECHA_ULTIMO_PAGO, CUOTA_REFUERZO, PAGO_A_CUENTA, PORCENTAJE_MORA, ID_MONEDA, NOMBRE_PARA_DOCUMENTO, ENTREGA_INICIAL, USER_AUTORIZADOR, FECHA_AUTORIZACION, HORA_AUTORIZACION, ID_JEFE_VENTA, NRO_DOCUMENTO_ENTREGA_INICIAL, RGP_NRO_SECCION, RGP_NUMERO, RGP_FOLIO, RGP_ANIO_INCRIPCION, TOTAL_DEVENGADO, ID_COMISION_JEFE_VENTA, FECHA_INCRIPCION_MUNIC, MES_ATRASO, ID_COMISION_PROPIETARIO, FECHA_VENTA, ID_ORDEN, MES_PARA_VENCIMIENTO, PRECIO_REAL_VENTA, IVA_CUOTA, ID_COMISION_CAPTADOR, NRO_RESOLUCION_MUNIC, FECHA_ORDEN_ESCRITURACION, ESTADO_CONTRATO, FECHA_ESTADO_CONTRATO, MENSAJE_PARA_CLIENTE, RGP_FECHA_INSCRIPCION, ID_GERENTE_VENTA, TIPO_VENTA, IMPORTE_ENTREGA_INICIAL, CUOTA_ENTREGA_INICIAL, FORMA_PAGO, ID_MEJORA, FRACCION, MANZANA, LOTE, CONTRATO_FIRMADO
                        FROM INMO.LOTES ORDER BY ID_LOTE ASC";


            return await db.QueryAsync<Inmo_Lote>(sql, new { });
        }
        public async Task<Inmo_Lote> GetLoteByID(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT CANT_DIAS_RESERVA, CODIGO_RESERVA, COSTO_LOTE, CTA_CTE_CTRAL, DIMENSION, DIMENSION_ESTE, DIMENSION_NORTE, DIMENSION_OESTE, DIMENSION_SUR, ESTADO, FECHA_CHEQUERA, FECHA_ESTADO, FECHA_INGRESO, ID_COMISION, ID_FRACCION, ID_LOTE, ID_MANZANA, LINDERO_ESTE, LINDERO_NORTE, LINDERO_OESTE, LINDERO_SUR, MONTO_CUOTA, NRO_FINCA, NUMERO_CONTRATO, OBSERVACION, PLAZO, PRECIO_CONTADO, PRECIO_FINANCIADO, ID_CLIENTE, ID_VENDEDOR, PRECIO_VENTA_FINAL, TOTAL_PAGADO, SALDO_DEUDOR, ULTIMA_CUOTA_PAGADA, FECHA_ULTIMO_PAGO, CUOTA_REFUERZO, PAGO_A_CUENTA, PORCENTAJE_MORA, ID_MONEDA, NOMBRE_PARA_DOCUMENTO, ENTREGA_INICIAL, USER_AUTORIZADOR, FECHA_AUTORIZACION, HORA_AUTORIZACION, ID_JEFE_VENTA, NRO_DOCUMENTO_ENTREGA_INICIAL, RGP_NRO_SECCION, RGP_NUMERO, RGP_FOLIO, RGP_ANIO_INCRIPCION, TOTAL_DEVENGADO, ID_COMISION_JEFE_VENTA, FECHA_INCRIPCION_MUNIC, MES_ATRASO, ID_COMISION_PROPIETARIO, FECHA_VENTA, ID_ORDEN, MES_PARA_VENCIMIENTO, PRECIO_REAL_VENTA, IVA_CUOTA, ID_COMISION_CAPTADOR, NRO_RESOLUCION_MUNIC, FECHA_ORDEN_ESCRITURACION, ESTADO_CONTRATO, FECHA_ESTADO_CONTRATO, MENSAJE_PARA_CLIENTE, RGP_FECHA_INSCRIPCION, ID_GERENTE_VENTA, TIPO_VENTA, IMPORTE_ENTREGA_INICIAL, CUOTA_ENTREGA_INICIAL, FORMA_PAGO, ID_MEJORA, FRACCION, MANZANA, LOTE, CONTRATO_FIRMADO
                        FROM INMO.LOTES WHERE ID_LOTE = :id";

            return await db.QueryFirstOrDefaultAsync<Inmo_Lote>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Inmo_Lote>> GetLotesByFracctionID(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT CANT_DIAS_RESERVA, CODIGO_RESERVA, COSTO_LOTE, CTA_CTE_CTRAL, DIMENSION, DIMENSION_ESTE, DIMENSION_NORTE, DIMENSION_OESTE, DIMENSION_SUR, ESTADO, FECHA_CHEQUERA, FECHA_ESTADO, FECHA_INGRESO, ID_COMISION, ID_FRACCION, ID_LOTE, ID_MANZANA, LINDERO_ESTE, LINDERO_NORTE, LINDERO_OESTE, LINDERO_SUR, MONTO_CUOTA, NRO_FINCA, NUMERO_CONTRATO, OBSERVACION, PLAZO, PRECIO_CONTADO, PRECIO_FINANCIADO, ID_CLIENTE, ID_VENDEDOR, PRECIO_VENTA_FINAL, TOTAL_PAGADO, SALDO_DEUDOR, ULTIMA_CUOTA_PAGADA, FECHA_ULTIMO_PAGO, CUOTA_REFUERZO, PAGO_A_CUENTA, PORCENTAJE_MORA, ID_MONEDA, NOMBRE_PARA_DOCUMENTO, ENTREGA_INICIAL, USER_AUTORIZADOR, FECHA_AUTORIZACION, HORA_AUTORIZACION, ID_JEFE_VENTA, NRO_DOCUMENTO_ENTREGA_INICIAL, RGP_NRO_SECCION, RGP_NUMERO, RGP_FOLIO, RGP_ANIO_INCRIPCION, TOTAL_DEVENGADO, ID_COMISION_JEFE_VENTA, FECHA_INCRIPCION_MUNIC, MES_ATRASO, ID_COMISION_PROPIETARIO, FECHA_VENTA, ID_ORDEN, MES_PARA_VENCIMIENTO, PRECIO_REAL_VENTA, IVA_CUOTA, ID_COMISION_CAPTADOR, NRO_RESOLUCION_MUNIC, FECHA_ORDEN_ESCRITURACION, ESTADO_CONTRATO, FECHA_ESTADO_CONTRATO, MENSAJE_PARA_CLIENTE, RGP_FECHA_INSCRIPCION, ID_GERENTE_VENTA, TIPO_VENTA, IMPORTE_ENTREGA_INICIAL, CUOTA_ENTREGA_INICIAL, FORMA_PAGO, ID_MEJORA, FRACCION, MANZANA, LOTE, CONTRATO_FIRMADO
                        FROM INMO.LOTES WHERE ID_FRACCION = :id";

            return await db.QueryAsync<Inmo_Lote>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Inmo_Lote>> GetLotesByClientID(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT l.CANT_DIAS_RESERVA, l.CODIGO_RESERVA, l.COSTO_LOTE, l.CTA_CTE_CTRAL, l.DIMENSION, l.DIMENSION_ESTE, l.DIMENSION_NORTE, l.DIMENSION_OESTE, l.DIMENSION_SUR, l.ESTADO, l.FECHA_CHEQUERA, l.FECHA_ESTADO, l.FECHA_INGRESO, l.ID_COMISION, l.ID_FRACCION, l.ID_LOTE, l.ID_MANZANA, l.LINDERO_ESTE, l.LINDERO_NORTE, l.LINDERO_OESTE, l.LINDERO_SUR, l.MONTO_CUOTA, l.NRO_FINCA, l.NUMERO_CONTRATO, l.OBSERVACION, l.PLAZO, l.PRECIO_CONTADO, l.PRECIO_FINANCIADO, l.ID_CLIENTE, l.ID_VENDEDOR, l.PRECIO_VENTA_FINAL, l.TOTAL_PAGADO, l.SALDO_DEUDOR, l.ULTIMA_CUOTA_PAGADA, l.FECHA_ULTIMO_PAGO, l.CUOTA_REFUERZO, l.PAGO_A_CUENTA, l.PORCENTAJE_MORA, l.ID_MONEDA, l.NOMBRE_PARA_DOCUMENTO, l.ENTREGA_INICIAL, l.USER_AUTORIZADOR, l.FECHA_AUTORIZACION, l.HORA_AUTORIZACION, l.ID_JEFE_VENTA, l.NRO_DOCUMENTO_ENTREGA_INICIAL, l.RGP_NRO_SECCION, l.RGP_NUMERO, l.RGP_FOLIO, l.RGP_ANIO_INCRIPCION, l.TOTAL_DEVENGADO, l.ID_COMISION_JEFE_VENTA, l.FECHA_INCRIPCION_MUNIC, l.MES_ATRASO, l.ID_COMISION_PROPIETARIO, l.FECHA_VENTA, l.ID_ORDEN, l.MES_PARA_VENCIMIENTO, l.PRECIO_REAL_VENTA, l.IVA_CUOTA, l.ID_COMISION_CAPTADOR, l.NRO_RESOLUCION_MUNIC, l.FECHA_ORDEN_ESCRITURACION, l.ESTADO_CONTRATO, l.FECHA_ESTADO_CONTRATO, l.MENSAJE_PARA_CLIENTE, l.RGP_FECHA_INSCRIPCION, l.ID_GERENTE_VENTA, l.TIPO_VENTA, l.IMPORTE_ENTREGA_INICIAL, l.CUOTA_ENTREGA_INICIAL, l.FORMA_PAGO, l.ID_MEJORA, l.FRACCION, l.MANZANA, l.LOTE, l.CONTRATO_FIRMADO
                        FROM INMO.LOTES l
                        INNER JOIN INMO.PROPIETARIO_LOTES pl  ON pl.ID_LOTE  = l.ID_LOTE AND pl.ID_FRACCION  = l.ID_FRACCION AND pl.ID_MANZANA = l.ID_MANZANA 
                        WHERE pl.ID_PERSONA  = :id";

            return await db.QueryAsync<Inmo_Lote>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Inmo_Pago>> GetPagos()
        {
            var db = dbConnection();
            var sql = @"SELECT ESTADO, FECHA_PAGO, FECHA_PROCESO, ID_FRACCION, ID_LOTE, ID_MANZANA, ID_MOVIMIENTO, IMPORTE_CHEQUE, IMPORTE_EFECTIVO, IMPORTE_MORA, NRO_COMPROBANTE, NUMERO_CUOTA, TIPO_CUOTA, TIPO_PAGO, ID_COBRADOR, MES_ATRAZO, TRANSFERIDO, IMPORTE_MORA_EXONERADA, ID_CAJA, ID_SUCURSAL, LIQUIDADO_VENDEDOR, FECHA_LIQ_VENDEDOR, LIQUIDADO_COBRADOR, FECHA_LIQ_COBRADOR, LIQUIDADO_PROPIETARIO, FECHA_LIQ_PROPIETARIO, LIQUIDADO_JEFE_VENTA, FECHA_LIQ_JEFE_VENTA, TOTAL_DEVENGADO, SECUENCIA, IMPORTE_CUOTA_EXONERADA, COMISION_PROP_CUOTA, COMISION_PROP_AJUSTE, IVA_CUOTAS, IVA_MORA, USUARIO_CARGADOR, TIPO_COMPROBANTE, LIQUIDADO_CAPTADOR, FECHA_LIQ_CAPTADOR, COMISION_CAPT_CUOTA, COMISION_CAPT_AJUSTE, TIPO_COBRO_VARIOS, IMPORTE_COBRO_VARIOS, FECHA_LIQ_GERENTE_VTA, LIQUIDADO_GERENTE_VTA, ID_COBRADOR_COMPARTIDO, PORC_COMISION_COB_COMP, NRO_CUOTA_INTERNA, NUMERO_CONTRATO, FECHA_REAL_PAGO
                        FROM INMO.PAGO_LOTES";

            return await db.QueryAsync<Inmo_Pago>(sql, new { });
        }


    }
}
