using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace GoldenGateAPI.Data
{
    public class Oraculo
    {

        public OracleConnection connection;

        public Oraculo()
        {

        }


        //DTO for execute generic query
        public DataTable GetData(string query, OracleConnection conn)
        {
            DataTable objresutl = new DataTable();

            try
            {

                OracleCommand command = new OracleCommand(query, conn);
                command.BindByName = true;

                OracleDataReader reader = command.ExecuteReader();

                objresutl.Load(reader);

                reader.Close();
             
            }
            catch (Exception ex)
            {
                return null;
            }

            return objresutl;
        }

        //DTO for consulting Cuotas 
        public DataTable GetConsultByTransactionCode(OracleConnection conn, params OracleParameter[] sqlParams)
        {
            DataTable objresutl = new DataTable();
            string str = "SELECT* from(SELECT CODIGO_RETORNO AS codRetorno, DESC_RETORNO AS desRetorno, NOMBRE_CLIENTE AS nombreApellido, ID_FRACCION AS fraccion, ID_MANZANA AS manzana, ID_LOTE AS lote, NUMERO_CONTRATO AS nroContrato, NUMERO_CUOTA AS nroCuota, MONTO_CUOTA AS montoCuota, MONTO_MORA AS montoMora, MESES_MORA mesesMora, FECHA_VENCIMIENTO AS fechaVencimiento, CODIGO_TRANS AS CodTransaccion FROM prn_cuotas WHERE CODIGO_TRANS = :CodTransaccion ORDER BY FECHA_PROCESO DESC, NUMERO_CUOTA asc) where ROWNUM <= 3";

            try
            {
               


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
              
            }
            catch (Exception ex)
            {
                return null;
            }

            return objresutl;
        }


        ~Oraculo()
        {
            connection.Close();
        }

    }
}
