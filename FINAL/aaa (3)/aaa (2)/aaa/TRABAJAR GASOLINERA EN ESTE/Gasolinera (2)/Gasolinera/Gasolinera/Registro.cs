using System;
using System.IO;
using System.Windows.Forms;

namespace gasolinera_json
{
    public partial class Registro : Form
    {
        private Super super;
        private Regular regular;
        private Diesel diesel;
        private ION_Diesel id;

        public Registro(Super super, Regular regular, Diesel diesel, ION_Diesel id)
        {
            InitializeComponent();
            this.super = super;
            this.regular = regular;
            this.diesel = diesel;
            this.id = id;
        }

        private void Registro_Load(object sender, EventArgs e)
        {
            ActualizarContador(super.ObtenerNumeroAbastecimientos(), super.ObtenerNumeroAbastecimientosFull(), regular.ObtenerNumeroAbastecimientos(),
           regular.ObtenerNumeroAbastecimientosFull(), diesel.ObtenerNumeroAbastecimientos(), diesel.ObtenerNumeroAbastecimientosFull(), id.ObtenerNumeroAbastecimientos(),
           id.ObtenerNumeroAbastecimientosFull());
            ActualizarTotalPrepago(); 
            ActualizarTotalFull();
            ActualizarTotalPrepagoR();
            ActualizarTotalFullR();
            ActualizarTotalFullD();
            ActualizarTotalPrepagoD();
        }

        private void label3_Click(object sender, EventArgs e) { }

        public void ActualizarContador(int contadorPrepSuper, int contadorFullSuper,int contadorPrepRegular,
            int cotadorFullRegular, int contadorPrepDiesel, int contadorFullDiesel, int contadorPrepId, int contadorFullId)
        {
            label3.Text = contadorPrepSuper.ToString();
            label9.Text = contadorFullSuper.ToString();
            label7.Text = contadorPrepRegular.ToString();
            label14.Text = cotadorFullRegular.ToString();
            label17.Text = contadorPrepDiesel.ToString();
            label20.Text = contadorFullDiesel.ToString();
            label17.Text = contadorPrepDiesel.ToString();
            label20.Text = contadorFullDiesel.ToString();
            label23.Text = contadorPrepId.ToString();
            label26.Text = contadorFullId.ToString();


        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Obtener los números de abastecimientos de todas las bombas
            int superPrep = super.ObtenerNumeroAbastecimientos();
            int superFull = super.ObtenerNumeroAbastecimientosFull();
            int regularPrep = regular.ObtenerNumeroAbastecimientos();
            int regularFull = regular.ObtenerNumeroAbastecimientosFull();
            int dieselPrep = diesel.ObtenerNumeroAbastecimientos();
            int dieselFull = diesel.ObtenerNumeroAbastecimientosFull();
            int idPrep = id.ObtenerNumeroAbastecimientos();
            int idFull = id.ObtenerNumeroAbastecimientosFull();

            // Calcular el total de abastecimientos para cada tipo de bomba
            int totalSuper = superPrep + superFull;
            int totalRegular = regularPrep + regularFull;
            int totalDiesel = dieselPrep + dieselFull;
            int totalId = idPrep + idFull;

            // Determinar la bomba más utilizada
            string bombaMasUtilizada = "Super";
            int maxUtilizacion = totalSuper;

            if (totalRegular > maxUtilizacion)
            {
                bombaMasUtilizada = "Regular";
                maxUtilizacion = totalRegular;
            }

            if (totalDiesel > maxUtilizacion)
            {
                bombaMasUtilizada = "Diesel";
                maxUtilizacion = totalDiesel;
            }

            if (totalId > maxUtilizacion)
            {
                bombaMasUtilizada = "ION Diesel";
                maxUtilizacion = totalId;
            }

            // Mostrar el resultado
            MessageBox.Show($"La bomba más utilizada es: {bombaMasUtilizada} con {maxUtilizacion} abastecimientos.");
        }


        private void ActualizarTotalPrepago() 
        {
           double totalPrepago = super.ObtenerTotalPrepago();
            label10.Text = totalPrepago.ToString("0.00");
        }
        private void ActualizarTotalFull() 
        {
            double totalFull = super.ObtenerTotalFull();
            label11.Text = totalFull.ToString("0.00");
        }
        private void ActualizarTotalPrepagoR()
        {
            double totalPrepago = regular.ObtenerTotalPrepago();
            label12.Text = totalPrepago.ToString("0.00");
        }
        private void ActualizarTotalFullR()
        {
            double totalFull = regular.ObtenerTotalFull();
            label15.Text = totalFull.ToString("0.00");
        }
        private void ActualizarTotalPrepagoD()
        {
            double totalPrepago = diesel.ObtenerTotalPrepago();
            label18.Text = totalPrepago.ToString("0.00");
        }
        private void ActualizarTotalFullD()
        {
            double totalFull = diesel.ObtenerTotalFull();
            label21.Text = totalFull.ToString("0.00");
        }
        private void ActualizarTotalPrepagoID()
        {
            double totalPrepago = diesel.ObtenerTotalPrepago();
            label18.Text = totalPrepago.ToString("0.00");
        }
        private void ActualizarTotalFullID()
        {
            double totalFull = diesel.ObtenerTotalFull();
            label21.Text = totalFull.ToString("0.00");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ruta = @"C:\Users\andre\Desktop\Prepago.dat";
            string ruta2 = @"C:\Users\andre\Desktop\tanqueLleno.dat";
            if (!File.Exists(ruta))
            {
                using (StreamWriter sr = File.CreateText(ruta))
                {
                    sr.WriteLine("-------------Informes de uso de bombas prepago---------------\n");
                    sr.WriteLine("Bomba super: " + super.ObtenerNumeroAbastecimientos() + "\n");
                    sr.WriteLine("Bomba regular: " + regular.ObtenerNumeroAbastecimientos() + "\n");
                    sr.WriteLine("Bomba diesel: " + diesel.ObtenerNumeroAbastecimientos() + "\n");
                    sr.WriteLine("Bomba ION diesel: " + id.ObtenerNumeroAbastecimientos() + "\n");

                }


            }
            else
            {
                File.Delete(ruta);
                using (StreamWriter sr = File.CreateText(ruta))
                {
                    sr.WriteLine("-------------Informes de uso de bombas prepago---------------\n");
                    sr.WriteLine("Bomba super: " + super.ObtenerNumeroAbastecimientos()+ "\n");
                    sr.WriteLine("Bomba regular: " + regular.ObtenerNumeroAbastecimientos() + "\n");
                    sr.WriteLine("Bomba diesel: " + diesel.ObtenerNumeroAbastecimientos() + "\n");
                    sr.WriteLine("Bomba ION diesel: " + id.ObtenerNumeroAbastecimientos() + "\n");


                }

            }
            
            if (!File.Exists(ruta2))
            {
                using (StreamWriter sr = File.CreateText(ruta2))
                {
                    sr.WriteLine("-------------Informes de uso de bombas tanque lleno---------------\n");
                    sr.WriteLine("Bomba super: " + super.ObtenerNumeroAbastecimientosFull() + "\n");
                    sr.WriteLine("Bomba regular: " + regular.ObtenerNumeroAbastecimientosFull() + "\n");
                    sr.WriteLine("Bomba diesel: " + diesel.ObtenerNumeroAbastecimientosFull() + "\n");
                    sr.WriteLine("Bomba diesel: " + id.ObtenerNumeroAbastecimientosFull() + "\n");

                }


            }
            else
            {
                File.Delete(ruta);
                using (StreamWriter sr = File.CreateText(ruta))
                {
                    sr.WriteLine("-------------Informes de uso de bombas tanque lleno---------------\n");
                    sr.WriteLine("Bomba super: " + super.ObtenerNumeroAbastecimientosFull() + "\n");
                    sr.WriteLine("Bomba regular: " + regular.ObtenerNumeroAbastecimientosFull() + "\n");
                    sr.WriteLine("Bomba diesel: " + diesel.ObtenerNumeroAbastecimientosFull() + "\n");
                    sr.WriteLine("Bomba diesel: " + id.ObtenerNumeroAbastecimientosFull() + "\n");


                }

            }
            MessageBox.Show("Los informes se han duardado exitosamente");
        }

    

    }
}
