using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Conexion
    {

        public string qcon = "Data source =localhost:1521/xe; password = 1234; user id = SIGLOXXI; ";

        public OracleConnection con = new OracleConnection("DATA SOURCE = localhost:1521/xe ; PASSWORD = 1234 ; USER ID = SIGLOXXI");

    }
}