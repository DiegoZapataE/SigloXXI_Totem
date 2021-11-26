using Controlador;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class ReservaDAO
    {
        Conexion c = new Conexion();
        public Reserva BuscarReservaPorFechaYRut(string fecha, int rut)
        {
            Reserva o = new Reserva();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_TOTEM.BUSCAR_RESERVAS_POR_FECHA_Y_RUT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_fecha_reserva", OracleDbType.NVarchar2).Value = fecha;
                    cmd.Parameters.Add("v_clientes_rut_cliente", OracleDbType.Int32).Value = rut;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        o.Id_Reserva = reader.GetInt32(0);
                        o.Fecha_Registro = reader.GetDateTime(1);
                        o.Fecha_Reserva = reader.GetDateTime(2);
                        o.Rut_Solicitante = reader.GetInt32(3);
                        o.Mesas_Id_Mesas = reader.GetInt32(4);
                        o.Horario_Reservas_Id_Horario_Reserva = reader.GetInt32(5);
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
        public Reserva BuscarMesasReservadas(string fecha, int id)
        {
            Reserva o = new Reserva();
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_TOTEM.BUSCAR_RESERVAS_POR_FECHA_Y_MESA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_fecha_reserva", OracleDbType.NVarchar2).Value = fecha;
                    cmd.Parameters.Add("v_mesas_id_mesa", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        o.Id_Reserva = reader.GetInt32(0);
                        o.Fecha_Registro = reader.GetDateTime(1);
                        o.Fecha_Reserva = reader.GetDateTime(2);
                        o.Rut_Solicitante = reader.GetInt32(3);
                        o.Mesas_Id_Mesas = reader.GetInt32(4);
                        o.Horario_Reservas_Id_Horario_Reserva = reader.GetInt32(5);
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
        public List<Reserva> BuscarReservasPorFecha(string fecha)
        {
            List<Reserva> lista = new List<Reserva>();
            
            try
            {
                using (OracleConnection con = new OracleConnection(c.qcon))
                {
                    OracleCommand cmd = new OracleCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "PKG_TOTEM.BUSCAR_RESERVAS_POR_FECHA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("v_fecha_reserva", OracleDbType.NVarchar2).Value = fecha;
                    cmd.Parameters.Add("v_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reserva o = new Reserva();
                        o.Id_Reserva = reader.GetInt32(0);
                        o.Fecha_Registro = reader.GetDateTime(1);
                        o.Fecha_Reserva = reader.GetDateTime(2);
                        o.Rut_Solicitante = reader.GetInt32(3);
                        o.Mesas_Id_Mesas = reader.GetInt32(4);
                        o.Horario_Reservas_Id_Horario_Reserva = reader.GetInt32(5);
                        lista.Add(o);
                    }
                    con.Close();
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return lista;
        }
    }
}
