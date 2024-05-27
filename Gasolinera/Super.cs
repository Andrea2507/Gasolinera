using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace gasolinera_json
{
    public partial class Super : Form
    {
        public int Contador { get; set; }
        public int Prepago { get; set; }
        public int TanqueLleno { get; set; }

        private int contador = 0;
        private int contador1 = 0;
        private Timer timer1;
        private Timer timer2;

        private List<Abastecimiento> abastecimientos = new List<Abastecimiento>();

        public Super()
        {
            InitializeComponent();
            InitializeTimers();

            // Configuración del puerto para Arduino
            /*
            arduino = new System.IO.Ports.SerialPort
            {
                PortName = "COM3",
                BaudRate = 9600
            };

            try
            {
                arduino.Open();
            }
            catch
            {
                MessageBox.Show("Asegurese que el arduino este conectado en COM3");
            }
            */
        }

        private void InitializeTimers()
        {
            timer1 = new Timer { Interval = 500 };
            timer1.Tick += Timer1_Tick;

            timer2 = new Timer { Interval = 3500 };
            timer2.Tick += Timer2_Tick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                Encender();
                IniciarTimers();
                AñadirAbastecimiento(txtNombre.Text);
                ActualizarListaAbastecimientos();

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

        private void Encender()
        {
            Contador++;
            //var dato = new { action = "Encender" };
            //string datoJson = JsonConvert.SerializeObject(dato);
            //arduino.Write(datoJson);
        }

        private void IniciarTimers()
        {
            timer1.Start();
            timer2.Start();
        }

        private void AñadirAbastecimiento(string nombreCliente)
        {
            abastecimientos.Add(new Abastecimiento(nombreCliente));
        }

        private void ActualizarListaAbastecimientos()
        {
            listBox1.Items.Clear();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            int numero = int.Parse(textBox1.Text);
            if (contador < numero)
            {
                contador++;
                label1.Text = contador.ToString();
            }
            else
            {
                DetenerTimers();
            }

            if (contador == numero)
            {
                var dato = new { action = "Apagar" };
                //string datoJson = JsonConvert.SerializeObject(dato);
                //arduino.Write(datoJson);
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            ReiniciarContadores();
        }

        private void ReiniciarContadores()
        {
            contador = 0;
            contador1 = 0;
            label1.Text = string.Empty;
            label5.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dato = new { action = "Apagar" };
            string datoJson = JsonConvert.SerializeObject(dato);
            //arduino.Write(datoJson);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            if (arduino.IsOpen)
            {
                arduino.Close();
            }
            */
        }

        private void DetenerTimers()
        {
            timer1.Stop();
            timer2.Stop();
        }

        public int ObtenerNumeroAbastecimientos()
        {
            return abastecimientos.Count;
        }
    }

}
