using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class AsignarMesa
    {
        public int Id_Asignacion { get; set; }
        public int Rut_Cliente { get; set; }
        public AsignarMesa()
        {
            this.Init();
        }

        private void Init()
        {
            Id_Asignacion = 0;
            Rut_Cliente = 0;
        }
    }
}
    
