using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Reserva
    {
        public int Id_Reserva { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public DateTime Fecha_Reserva { get; set; }
        public int Rut_Solicitante { get; set; }
        public int Mesas_Id_Mesas { get; set; }
        public int Horario_Reservas_Id_Horario_Reserva { get; set; }

        public Reserva()
        {
            this.Init();
        }

        private void Init()
        {
            Id_Reserva = 0;
            Rut_Solicitante = 0;
            Mesas_Id_Mesas = 0;
            Horario_Reservas_Id_Horario_Reserva = 0;
        }
    }
}
