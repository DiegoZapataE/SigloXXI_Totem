using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace TotemSigloXXI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AsignarMesaDAO dao = new AsignarMesaDAO();
        int cantidad_solicitudes =0;
        public MainWindow()
        {
            InitializeComponent();
            txtResultado.IsReadOnly = true;
            txtInfo.IsReadOnly = true;
            DispatcherTimer timer = new DispatcherTimer();
            DispatcherTimer refresh = new DispatcherTimer();
            DispatcherTimer reservas = new DispatcherTimer();

            timer.Tick += new EventHandler(ActualizarTimer);
            timer.Interval = new TimeSpan(0,0,1);
            timer.Start();

            refresh.Tick += new EventHandler(ComprobarSolicitudes);
            refresh.Interval = new TimeSpan(0, 0, 5);
            refresh.Start();
            cantidad_solicitudes = dao.BuscarUltimaSolicitud().Id_Asignacion;
            txtRut.Focus();

            reservas.Tick += new EventHandler(ActualizarMesasReservadas);
            reservas.Interval = new TimeSpan(0, 0, 5);
            reservas.Start();
        }

        private void ActualizarTimer(object sender, EventArgs e)
        {
            txtHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }
        private void Limpiar_Campos(object sender, EventArgs e)
        {
            txtResultado.Text = "";
            txtRut.Text = "";
            txtRut.Focus();
        }
        public async void ActualizarPantalla()
        {
            LimpiarRut();
            await Task.Delay(6000);
            txtResultado.Text = "";
            txtRut.Focus();
        }
        public async void LimpiarRut()
        {
            await Task.Delay(1000);
            txtRut.Text = "";
        }

        private void ComprobarSolicitudes(object sender, EventArgs e)
        {
            AsignarMesaDAO dao = new AsignarMesaDAO();
            if (cantidad_solicitudes != dao.BuscarUltimaSolicitud().Id_Asignacion)
            {
                cantidad_solicitudes = dao.BuscarUltimaSolicitud().Id_Asignacion;
                ReservaDAO rDAO = new ReservaDAO();
                MesaDAO mDAO = new MesaDAO();
                string fecha_actual = DateTime.Now.ToString("dd'-'MM'-'yyyy");
                int rut = dao.BuscarUltimaSolicitud().Rut_Cliente;

                if (fecha_actual != rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Fecha_Reserva.ToString("dd'-'MM'-'yyyy") && mDAO.TraerMesaPorRut(rut).Clientes_Rut_Cliente != rut)
                {
                    int mesa_disponible = mDAO.TraerMesaPorRut(-999).Id_Mesa;
                    if (mDAO.TraerMesaPorRut(-999).Clientes_Rut_Cliente == -999)
                    {
                        mDAO.ActualizarMesa(mesa_disponible, 3, rut);
                        txtResultado.Text = "Se te ha asignado la mesa número " + mDAO.TraerMesaPorRut(rut).Id_Mesa + ". ¡Disfruta!";
                        ActualizarPantalla();
                    }
                    else
                    {
                        txtResultado.Text = "Lo lamentamos. Todas las mesas se encuentran ocupadas de momento. Diríjase a recepción para recibir más información.";
                        ActualizarPantalla();
                    }
                }
                else if (fecha_actual == rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Fecha_Reserva.ToString("dd'-'MM'-'yyyy") | mDAO.TraerMesaPorRut(rut).Clientes_Rut_Cliente == rut)
                {
                    if (mDAO.TraerMesaPorRut(rut).Clientes_Rut_Cliente == rut)
                    {
                        txtResultado.Text = "Tu mesa es la número " + mDAO.TraerMesaPorRut(rut).Id_Mesa + ". ¡Disfruta!";
                        int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                        mDAO.ActualizarMesa(id, 3, rut);
                        ActualizarPantalla();
                    }
                    else if (fecha_actual == rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Fecha_Reserva.ToString("dd'-'MM'-'yyyy"))
                    {
                        int horario = rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Horario_Reservas_Id_Horario_Reserva;
                        int hora_actual = int.Parse(DateTime.Now.ToString("HH"));
                        if (horario == 1)
                        {
                            if (10 <= hora_actual && hora_actual < 13)
                            {
                                txtResultado.Text = "Tu mesa es la número " + rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Mesas_Id_Mesas + ". ¡Disfruta!";
                                int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                                mDAO.ActualizarMesa(id, 3, rut);
                                ActualizarPantalla();
                            }
                            else
                            {
                                txtResultado.Text = "Tienes una reserva ingresada para hoy entre las 10:00 - 13:00. Por favor verifica en nuestra página web.";
                                ActualizarPantalla();
                            }
                        }
                        else if (horario == 2)
                        {
                            if (15 <= hora_actual && hora_actual < 18)
                            {
                                txtResultado.Text = "Tu mesa es la número " + rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Mesas_Id_Mesas + ". ¡Disfruta!";
                                int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                                mDAO.ActualizarMesa(id, 3, rut);
                                ActualizarPantalla();
                            }
                            else
                            {
                                txtResultado.Text = "Tienes una reserva ingresada para hoy entre las 15:00 - 18:00. Por favor verifica en nuestra página web.";
                                ActualizarPantalla();
                            }

                        }
                        else if (horario == 3)
                        {
                            if (20 <= hora_actual && hora_actual < 23)
                            {
                                txtResultado.Text = "Tu mesa es la número " + rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Mesas_Id_Mesas + ". ¡Disfruta!";
                                int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                                mDAO.ActualizarMesa(id, 3, rut);
                                ActualizarPantalla();
                            }
                            else
                            {
                                txtResultado.Text = "Tienes una reserva ingresada para hoy entre las 20:00 - 23:00. Por favor verifica en nuestra página web.";
                                ActualizarPantalla();
                            }
                        }
                        else
                        {
                            txtResultado.Text = "Tienes una reserva ingresada, pero no se ha logrado identificar el horario. Por favor diríjase a recepción.";
                            ActualizarPantalla();
                        }
                    }
                }
                else
                {
                    txtResultado.Text = "Se ha producido un error. Por favor diríjase a recepción.";
                    ActualizarPantalla();
                }
            }
        }

        private void txtResultado_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (txtRut.Text.Length < 11)
            {
                MessageBox.Show("Por favor ingresa un rut del tamaño correcto, incluyendo tu dígito verificador.");
            }
            else
            {
                ReservaDAO rDAO = new ReservaDAO();
                MesaDAO mDAO = new MesaDAO();
                string fecha_actual = DateTime.Now.ToString("dd'-'MM'-'yyyy");
                string rut_completo = txtRut.Text;
                rut_completo = rut_completo.Replace("-", String.Empty);
                rut_completo = rut_completo.Replace(".", String.Empty);
                int rut = int.Parse(rut_completo.Substring(0, rut_completo.Length - 1));

                if (fecha_actual != rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Fecha_Reserva.ToString("dd'-'MM'-'yyyy") && mDAO.TraerMesaPorRut(rut).Clientes_Rut_Cliente != rut)
                {
                    int mesa_disponible = mDAO.TraerMesaPorRut(-999).Id_Mesa;
                    if (mDAO.TraerMesaPorRut(-999).Clientes_Rut_Cliente == -999)
                    {
                        mDAO.ActualizarMesa(mesa_disponible, 3, rut);
                        txtResultado.Text = "Se te ha asignado la mesa número " + mDAO.TraerMesaPorRut(rut).Id_Mesa + ". ¡Disfruta!";
                        ActualizarPantalla();
                    }
                    else
                    {
                        txtResultado.Text = "Lo lamentamos. Todas las mesas se encuentran ocupadas de momento. Diríjase a recepción para recibir más información.";
                        ActualizarPantalla();
                    }
                }
                else if (fecha_actual == rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Fecha_Reserva.ToString("dd'-'MM'-'yyyy") | mDAO.TraerMesaPorRut(rut).Clientes_Rut_Cliente == rut)
                {
                    if (mDAO.TraerMesaPorRut(rut).Clientes_Rut_Cliente == rut)
                    {
                        txtResultado.Text = "Tu mesa es la número " + mDAO.TraerMesaPorRut(rut).Id_Mesa + ". ¡Disfruta!";
                        int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                        mDAO.ActualizarMesa(id, 3, rut);
                        ActualizarPantalla();
                    }
                    else if (fecha_actual == rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Fecha_Reserva.ToString("dd'-'MM'-'yyyy"))
                    {
                        int horario = rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Horario_Reservas_Id_Horario_Reserva;
                        int hora_actual = int.Parse(DateTime.Now.ToString("HH"));
                        if (horario == 1)
                        {
                            if (10 <= hora_actual && hora_actual < 13)
                            {
                                txtResultado.Text = "Tu mesa es la número " + rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Mesas_Id_Mesas + ". ¡Disfruta!";
                                int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                                mDAO.ActualizarMesa(id, 3, rut);
                                ActualizarPantalla();
                            }
                            else
                            {
                                txtResultado.Text = "Tienes una reserva ingresada para hoy entre las 10:00 - 13:00. Por favor verifica en nuestra página web.";
                                ActualizarPantalla();
                            }
                        }
                        else if (horario == 2)
                        {
                            if (15 <= hora_actual && hora_actual < 18)
                            {
                                txtResultado.Text = "Tu mesa es la número " + rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Mesas_Id_Mesas + ". ¡Disfruta!";
                                int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                                mDAO.ActualizarMesa(id, 3, rut);
                                ActualizarPantalla();
                            }
                            else
                            {
                                txtResultado.Text = "Tienes una reserva ingresada para hoy entre las 15:00 - 18:00. Por favor verifica en nuestra página web.";
                                ActualizarPantalla();
                            }

                        }
                        else if (horario == 3)
                        {
                            if (20 <= hora_actual && hora_actual < 23)
                            {
                                txtResultado.Text = "Tu mesa es la número " + rDAO.BuscarReservaPorFechaYRut(fecha_actual, rut).Mesas_Id_Mesas + ". ¡Disfruta!";
                                int id = mDAO.TraerMesaPorRut(rut).Id_Mesa;
                                mDAO.ActualizarMesa(id, 3, rut);
                                ActualizarPantalla();
                            }
                            else
                            {
                                txtResultado.Text = "Tienes una reserva ingresada para hoy entre las 20:00 - 23:00. Por favor verifica en nuestra página web.";
                                ActualizarPantalla();
                            }
                        }
                        else
                        {
                            txtResultado.Text = "Tienes una reserva ingresada, pero no se ha logrado identificar el horario. Por favor diríjase a recepción.";
                            ActualizarPantalla();
                        }
                    }
                }
                else
                {
                    txtResultado.Text = "Se ha producido un error. Por favor diríjase a recepción.";
                    ActualizarPantalla();
                }
            }
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ActualizarMesasReservadas(object sender, EventArgs e)
        {
            ReservaDAO rDAO = new ReservaDAO();
            MesaDAO mDAO = new MesaDAO();
            string fecha_actual = DateTime.Now.ToString("dd'-'MM'-'yyyy");
            int hora_actual = int.Parse(DateTime.Now.ToString("HH"));
            foreach (var item in rDAO.BuscarReservasPorFecha(fecha_actual))
            {
                if(fecha_actual == item.Fecha_Reserva.ToString("dd'-'MM'-'yyyy"))
                {
                    int horario = item.Horario_Reservas_Id_Horario_Reserva;
                    if (horario == 1)
                    {
                        if (10 <= hora_actual && hora_actual < 13)
                        {
                            int rut = item.Rut_Solicitante;
                            int id = item.Mesas_Id_Mesas;
                            mDAO.ActualizarMesa(id, 2, rut);
                        }
                    }
                    else if (horario == 2)
                    {
                        if (15 <= hora_actual && hora_actual < 18)
                        {
                            int rut = item.Rut_Solicitante;
                            int id = item.Mesas_Id_Mesas;
                            mDAO.ActualizarMesa(id, 2, rut);
                        }

                    }
                    else if (horario == 3)
                    {
                        if (20 <= hora_actual && hora_actual < 23)
                        {
                            int rut = item.Rut_Solicitante;
                            int id = item.Mesas_Id_Mesas;
                            mDAO.ActualizarMesa(id, 2, rut);
                        }
                    }
                }
            }
        }
        private void txtRut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (e.KeyChar.ToString().ToUpper().Equals("K"))
            {
                e.Handled = false;
            }
            else if (e.KeyChar.ToString().ToUpper().Equals("-"))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void txtRut_KeyUp(object sender, KeyEventArgs e)
        {
            txtRut.Text = FormatearRut(txtRut.Text);

            txtRut.SelectionStart = txtRut.Text.Length;
            txtRut.SelectionLength = 0;
        }

        public static string FormatearRut(string rut)
        {
            string rutFormateado = string.Empty;

            if (rut.Length == 0)
            {
                rutFormateado = "";
            }
            else
            {
                string rutTemporal;
                string dv;
                int rutNumerico;

                rut = rut.Replace("-", "").Replace(".", "");

                if (rut.Length == 1)
                {
                    rutFormateado = rut;
                }
                else
                {
                    rutTemporal = rut.Substring(0, rut.Length - 1);
                    dv = rut.Substring(rut.Length - 1, 1);

                    if (!int.TryParse(rutTemporal, out rutNumerico))
                    {
                        rutNumerico = 0;
                    }

                    rutFormateado = rutNumerico.ToString("N0");

                    if (rutFormateado.Equals("0"))
                    {
                        rutFormateado = string.Empty;
                    }
                    else
                    {
                        rutFormateado += "-" + dv;
                        rutFormateado = rutFormateado.Replace(",", ".");
                    }
                }
            }

            return rutFormateado;
        }

        private void btnInvitado_Click(object sender, RoutedEventArgs e)
        {
            MesaDAO mDAO = new MesaDAO();
            if(mDAO.TraerMesaPorRut(-999).Clientes_Rut_Cliente == -999)
            {
                int id = mDAO.TraerMesaPorRut(-999).Id_Mesa;
                mDAO.ActualizarMesa(id, 3, id);
                txtResultado.Text = "Se te ha asignado la mesa número " + id + ". ¡Disfruta!";
                ActualizarPantalla();
            }
            else
            {
                txtResultado.Text = "Lo sentimos. No existen mesas disponibles. Por favor comunicarse con recepción";
                ActualizarPantalla();
            }
        }
    }
}
