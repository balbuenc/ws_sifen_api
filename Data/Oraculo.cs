using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace GoldenGateAPI.Data
{
    public class Oraculo
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        private OracleConnection conn;

        public Oraculo(ILogger logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        //DTO for execute generic query
        public DataTable GetData(string str, string oracleDataSource)
        {
            DataTable objresutl = new DataTable();

            try
            {
                conn = new OracleConnection(oracleDataSource);
                conn.Open();


                OracleCommand command = new OracleCommand(str, conn);
                command.BindByName = true;

                OracleDataReader reader = command.ExecuteReader();

                objresutl.Load(reader);

                reader.Close();
                conn.Close();

                _logger.LogInformation("[{0}][Oracle] GetData(..), Executed Correctly. ",DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("[{0}][Oracle] GetData(..), ERROR: {1}.", DateTime.Now.ToString(), ex.Message);
                return null;
            }
            return objresutl;
        }

        //DTO for consulting Cuotas 
        public DataTable GetConsultByTransactionCode(string oracleDataSource, params OracleParameter[] sqlParams)
        {
            DataTable objresutl = new DataTable();
            string str = "SELECT* from(SELECT CODIGO_RETORNO AS codRetorno, DESC_RETORNO AS desRetorno, NOMBRE_CLIENTE AS nombreApellido, ID_FRACCION AS fraccion, ID_MANZANA AS manzana, ID_LOTE AS lote, NUMERO_CONTRATO AS nroContrato, NUMERO_CUOTA AS nroCuota, MONTO_CUOTA AS montoCuota, MONTO_MORA AS montoMora, MESES_MORA mesesMora, FECHA_VENCIMIENTO AS fechaVencimiento, CODIGO_TRANS AS CodTransaccion FROM prn_cuotas WHERE CODIGO_TRANS = :CodTransaccion ORDER BY FECHA_PROCESO DESC, NUMERO_CUOTA asc) where ROWNUM <= 3";

            try
            {
                conn = new OracleConnection(oracleDataSource);
                conn.Open();


                OracleCommand command = new OracleCommand(str, conn);
                command.BindByName = true;

                if (sqlParams != null)
                {
                    foreach (OracleParameter para in sqlParams)
                    {
                        command.Parameters.Add(para);
                    }
                }

                OracleDataReader reader = command.ExecuteReader();

                objresutl.Load(reader);

                reader.Close();
                conn.Close();

                _logger.LogInformation("[{0}][Oracle] GetData(..), Executed Correctly. ", DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("[{0}][Oracle] GetData(..), ERROR: {1}.", DateTime.Now.ToString(), ex.Message);
                return null;
            }
            return objresutl;
        }


        ~Oraculo()
        {

            conn.Close();

        }

    }
}
