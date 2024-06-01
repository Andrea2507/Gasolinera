using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace gasolinera_json
{
    public partial class Super : Form
    {
        public int Contador { get; set; }
        public int Prepago { get; set; }
        public int TanqueLleno { get; set; }

        private const int INTERVALO_MILISEGUNDOS = 100;
        private Timer timer1;
        private Timer timer2;
        private double PrecioLitro = 10.0;
        private int contador1;
        private List<Abastecimiento> abastecimientos = new List<Abastecimiento>();

        private SerialPort arduino;

        public Super()
        {
            InitializeComponent();
            InitializeTimers();

            arduino = new SerialPort
            {
                PortName = "COM3",
                BaudRate = 9600
            };

            try
            {
                arduino.Open();
                arduino.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            }
            catch
            {
                MessageBox.Show("Asegúrese de que el arduino esté conectado en COM3");
                Environment.Exit(0);
            }
        }

        private void InitializeTimers()
        {
            timer1 = new Timer { Interval = INTERVALO_MILISEGUNDOS };
            timer1.Tick += Timer1_Tick;

            timer2 = new Timer { Interval = INTERVALO_MILISEGUNDOS };
            timer2.Tick += Timer2_Tick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                double cantidadQuetzales;
                if (double.TryParse(textBox1.Text, out cantidadQuetzales) && cantidadQuetzales > 0)
                {
                    double litros = cantidadQuetzales / PrecioLitro;
                    EncenderMotor();
                    IniciarTimers();
                    AñadirAbastecimiento(txtNombre.Text);
                    ActualizarListaAbastecimientos();

                    label8.Text = $"{PrecioLitro} Q por litro";
                    label15.Text = $" Total litros: {litros} litros";
                }
                else
                {
                    MessageBox.Show("Por favor ingrese una cantidad válida en quetzales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese la cantidad en quetzales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TanqueLleno = 1;
             var command = new { action = "Lleno" };
            string jsonCommand = JsonConvert.SerializeObject(command);
            arduino.WriteLine(jsonCommand);
            IniciarTimerTanqueLleno();
        }

        private void IniciarTimerTanqueLleno()
        {
            timer2.Start();
        }

        private void EncenderMotor()
        {
            var command = new { action = "Lleno" };
            string jsonCommand = JsonConvert.SerializeObject(command);
            arduino.WriteLine(jsonCommand);
        }

        private void ApagarMotor()
        {
            var command = new { action = "Apagar" };
            string jsonCommand = JsonConvert.SerializeObject(command);
            arduino.WriteLine(jsonCommand);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string message = arduino.ReadLine();
                this.Invoke(new Action(() =>
                {
                   
                    if (message.Contains("Motor detenido"))
                    {
                        DetenerTimers();
                        ApagarMotor();
                    }
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    label10.Text = "Error: " + ex.Message;
                }));
            }
        }

        private void IniciarTimers()
        {
            timer1.Start();
        }

        private void DetenerTimers()
        {
            timer1.Stop();
            timer2.Stop();
        }

        private void AñadirAbastecimiento(string nombreCliente)
        {
            abastecimientos.Add(new Abastecimiento(nombreCliente));
        }

        private void ActualizarListaAbastecimientos()
        {
            listBox1.Items.Clear();
            foreach (var abastecimiento in abastecimientos)
            {
                listBox1.Items.Add($"Fecha: {abastecimiento.Fecha.ToShortDateString()} - Hora: {abastecimiento.Hora} - Cliente: {abastecimiento.NombreCliente}");
            }
        }

        private double contadorLitros = 0.0;

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double cantidadDeseada))
            {
                double incremento = 1.0 / (14000 / INTERVALO_MILISEGUNDOS);

                if (contadorLitros < cantidadDeseada / PrecioLitro)
                {
                    contadorLitros += incremento;
                    double costoTotal = contadorLitros * PrecioLitro;
                    label9.Text = $"Costo total: {costoTotal.ToString("0.00")} Q";
                    label1.Text = contadorLitros.ToString("0.00");
                    double vuelto = cantidadDeseada - costoTotal;
                    label13.Text = $" vuelto : {vuelto.ToString("0.0")} Q";
                  
                    if (contadorLitros >= cantidadDeseada / PrecioLitro)
                    {
                        contador1++;

                        DetenerTimers();
                        ApagarMotor();
                    }
                }
                else
                {
                    DetenerTimers();
                    ApagarMotor();
                }
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            double incremento = 1.0 / (14000 / INTERVALO_MILISEGUNDOS);
            contadorLitros += incremento;
            label5.Text = contadorLitros.ToString("0.00");

            double costoTotal = contadorLitros * PrecioLitro;
            label10.Text = $"Costo total: {costoTotal.ToString("0.00")} Q";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ReiniciarContadores();
        }

        private void ReiniciarContadores()
        {
            Contador = 0;
            contador1 = 0;
            contadorLitros = 0.0;
            label1.Text = string.Empty;
            label5.Text = string.Empty;
            label8.Text = string.Empty;
            label9.Text = string.Empty;
            timer1.Stop();
            timer2.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (arduino.IsOpen)
            {
                arduino.Close();
            }
        }

        public int ObtenerNumeroAbastecimientos()
        {
            return abastecimientos.Count;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que quiere cambiar el precio? Al elegir 'Sí', se eliminarán los datos del día y se reiniciará el contador.", "Confirmación de Cambio de Precio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string precioNuevo = Interaction.InputBox("Ingrese el nuevo precio por litro en quetzales:", "Nuevo Precio", PrecioLitro.ToString());

                if (double.TryParse(precioNuevo, out double nuevoPrecio) && nuevoPrecio > 0)
                {
                    PrecioLitro = nuevoPrecio;
                    label8.Text = $"{PrecioLitro} Q por litro";
                    MessageBox.Show($"El precio ha sido actualizado a {PrecioLitro}Q por litro.", "Precio Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReiniciarContadores();
                    abastecimientos.Clear();
                    listBox1.Items.Clear();
                }
                else
                {
                    MessageBox.Show("No se realizaron cambios", "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e) { }

        private void button2_Click_2(object sender, EventArgs e)
        {
            ApagarMotor();
            DetenerTimers();
        }

    }
}


