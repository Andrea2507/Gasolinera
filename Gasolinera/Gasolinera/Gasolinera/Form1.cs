using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace gasolinera_json
{
    
    public partial class Form1 : Form
    {
        System.IO.Ports.SerialPort arduino;
        List<Abastecimiento> abastecimientos = new List<Abastecimiento>();
        public Form1()
        {
            InitializeComponent();

            arduino = new System.IO.Ports.SerialPort();
            arduino.PortName = "COM3";
            arduino.BaudRate = 9600;

            try
            {
                arduino.Open();
            }
            catch
            {
                MessageBox.Show("Asegurese que el arduino este conectado en COM3");
                //Environment.Exit(1);
            }
        }
        int contador = 0;
        int contador1 = 0;
        int contadorBomba1 = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                var dato = new { action = "Encender" };
                contadorBomba1++;

                string datoJson = JsonConvert.SerializeObject(dato);

                arduino.Write(datoJson);

                timer1 = new Timer();
                timer1.Interval = 500;
                timer1.Tick += timer1_Tick;

                timer1.Start();

                timer2 = new Timer();
                timer2.Interval = 3500;
                timer2.Tick += timer2_Tick;

                timer2.Start();
                
                string nombreCliente = txtNombre.Text;
                abastecimientos.Add(new Abastecimiento(nombreCliente));
                listBox1.Items.Clear();

             
                foreach (var abastecimiento in abastecimientos)
                {
                    listBox1.Items.Add($"Fecha: {abastecimiento.Fecha.ToShortDateString()} - Hora: {abastecimiento.Hora} - Cliente: {abastecimiento.NombreCliente}");
                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese la cantidad o seleccione el boton tanque lleno");
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(arduino.IsOpen)
            {

                arduino.Close();
            }
        }

      

        private void timer1_Tick(object sender, EventArgs e)
        {
           int numero=int.Parse(textBox1.Text);
            if (contador < numero)
            {

                contador++;
                label1.Text = contador.ToString();
            }
            else
            { 
                timer1.Stop();
                timer2.Stop();
                
            }
               
            if(contador == numero) {

                var dato = new { action = "Apagar" };

                string datoJson = JsonConvert.SerializeObject(dato);

                // Enviar el JSON al Arduino
                arduino.Write(datoJson);
            }
            else { }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            contador = 0;
           
            label1.Text = "";
            label5.Text = "";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int numero2 = int.Parse(label1.Text);

            if (contador1 < numero2)
            {

                contador1 += 3;

            
                label5.Text = contador1.ToString();
            }
            else
            {
                
                timer2.Stop();
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {

            var dato = new { action = "Apagar" };
            string DatoJson = JsonConvert.SerializeObject(dato);
            arduino.Write(DatoJson);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

 }

