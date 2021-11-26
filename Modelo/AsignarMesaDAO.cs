using Controlador;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class AsignarMesaDAO
    {
        Conexion c = new Conexion();
        public AsignarMesa BuscarUltimaSolicitud()
        {
            AsignarMesa o = new AsignarMesa();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_TOTEM.BUSCAR_ULTIMA_SOLICITUD";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        o.Id_Asignacion = reader.GetInt32(0);
                        o.Rut_Cliente = reader.GetInt32(1);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return o;
        }
    }
}
