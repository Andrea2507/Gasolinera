using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
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
        int contador1;
        private List<Abastecimiento> abastecimientos = new List<Abastecimiento>();

        // System.IO.Ports.SerialPort arduino;

        public Super()
        {
            InitializeComponent();
            InitializeTimers();

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
                Environment.Exit(0);
            }
            */
        }

        private void InitializeTimers()
        {
            timer1 = new Timer { Interval = INTERVALO_MILISEGUNDOS };
            timer1.Tick += Timer1_Tick;

            timer2 = new Timer { Interval = 5000 }; 
            timer2.Tick += Timer2_Tick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    if (double.TryParse(textBox1.Text, out double cantidadQuetzales) && cantidadQuetzales > 0)
                    {
                        double litros = cantidadQuetzales / PrecioLitro;
                        Encender();
                        ReiniciarContadores(); 
                        IniciarTimers();
                        AñadirAbastecimiento(txtNombre.Text);
                        ActualizarListaAbastecimientos();

                        foreach (var abastecimiento in abastecimientos)
                        {
                            listBox1.Items.Add($"Fecha: {abastecimiento.Fecha.ToShortDateString()} - Hora: {abastecimiento.Hora} - Cliente: {abastecimiento.NombreCliente}");
                        }

                        label8.Text = $"{PrecioLitro} Q por litro";
                        label9.Text = $"Total a pagar: {cantidadQuetzales} Q por {litros} litros";
                      
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

        private void Encender()
        {
            try
            {
                Contador++;
                var dato = new { action = "Encender" };
                string datoJson = JsonConvert.SerializeObject(dato);
                // arduino.Write(datoJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al encender el dispositivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private double contadorLitros = 0.0;
       

       

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
                if (double.TryParse(textBox1.Text, out double cantidadDeseada))
                {
               
                    double incremento = 1.0 / (5000 / INTERVALO_MILISEGUNDOS);

                    if (contadorLitros < cantidadDeseada/PrecioLitro)
                    {
                        contadorLitros += incremento;
                        label1.Text = contadorLitros.ToString("0.00"); 

                        if (contadorLitros >= cantidadDeseada/PrecioLitro)
                        {
                            contador1++;
                            label5.Text = contador1.ToString();
                            DetenerTimers();
                        }
                    }
                    else
                    {
                        DetenerTimers();
                        var dato = new { action = "Apagar" };
                        string datoJson = JsonConvert.SerializeObject(dato);
                        // arduino.Write(datoJson);
                    }
                }
                
            
           
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            
                if (double.TryParse(label1.Text, out double litros))
                {
                    if (contador1 < litros)
                    {
                        contador1++;
                        label5.Text = contador1.ToString();
                    }
                    else
                    {
                        timer2.Stop();
                    }
                }
               
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

        private void button2_Click(object sender, EventArgs e)
        {
            var dato = new { action = "Apagar" };
            string datoJson = JsonConvert.SerializeObject(dato);
            // arduino.Write(datoJson);
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

        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que quiere cambiar el precio? Al elegir 'Sí', se eliminarán los datos del día y se reiniciará el contador.", "Confirmación de Cambio de Precio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string precioNuevo = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el nuevo precio por litro en quetzales:", "Nuevo Precio", PrecioLitro.ToString());

                if (double.TryParse(precioNuevo, out double nuevoPrecio) && nuevoPrecio > 0)
                {
                    PrecioLitro = nuevoPrecio;
                    label8.Text = $"{PrecioLitro} Q por litro";
                    MessageBox.Show($"El precio ha sido actualizado a {PrecioLitro} Q por litro.", "Precio Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

   
}
