using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace gasolinera_json
{
    public partial class ION_Diesel : Form
    {
        public int Contador { get; set; }
        public int Prepago { get; set; }
        private int TanqueLlenoSuper { get; set; }

        private const int INTERVALO_MILISEGUNDOS = 100;
        private Timer timer1;
        private Timer timer2;
        private double PrecioLitro = 10.0;
        private int contador1;
        private List<Abastecimiento> abastecimientos = new List<Abastecimiento>();
        private List<Abastecimiento> abastecimientosFull = new List<Abastecimiento>();
        private static SerialPort arduino;
        private double totalPrepago = 0;
        private double totalFull = 0;

        public ION_Diesel()
        {
            InitializeComponent();
            InitializeTimers();
            label8.Text = $"{PrecioLitro} Q por litro";
            dataGridView1.AutoGenerateColumns = true;
            arduino = SerialManager.Arduino;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SerialManager.AbrirPuertoSerial();
            arduino.DataReceived += SerialPort_DataReceived;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
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
            if (contadorLitros > 0)
            {
                MessageBox.Show("La bomba no está en 0. Por favor, reinicie la bomba antes de continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (double.TryParse(textBox1.Text, out double cantidadQuetzales) && cantidadQuetzales > 0)
                {
                    double litros = cantidadQuetzales / PrecioLitro;
                    EncenderMotor();
                    IniciarTimers();
                    AñadirAbastecimiento(txtNombre.Text);
                    ActualizarListaAbastecimientos();

                    label15.Text = $" {litros} litros";
                    button4.Enabled = false;
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
            if (contadorLitros > 0)
            {
                MessageBox.Show("La bomba no está en 0. Por favor, reinicie la bomba antes de continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TanqueLlenoSuper = 1;
            EnviarComando(new { action = "LlenoIonDiesel" });
            AñadirAbastecimientoFull(txtNombre.Text);
            ActualizarListaAbastecimientosFull();
            IniciarTimerTanqueLleno();
            button1.Enabled = false;
        }

        private void IniciarTimerTanqueLleno()
        {
            timer2.Start();
        }

        private void EncenderMotor()
        {
            EnviarComando(new { action = "LlenoIonDiesel" });
        }

        private void ApagarMotor()
        {
            EnviarComando(new { action = "Apagar4" });
        }

        private void EnviarComando(object command)
        {
            string jsonCommand = JsonConvert.SerializeObject(command);


            if (arduino.IsOpen)
            {
                arduino.WriteLine(jsonCommand);
            }

        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string message = arduino.ReadLine();
                this.Invoke(new Action(() =>
                {

                    if (message.Contains("Motor detenido4"))
                    {
                        DetenerTimers();
                        ApagarMotor();
                        MostrarMensajeTanqueLleno();
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

        private void MostrarMensajeTanqueLleno()
        {
            double costoTotal = contadorLitros * PrecioLitro;
            label19.Text = $"El tanque está lleno, el total es: {costoTotal.ToString("0.00")} Q";
            if (TanqueLlenoSuper == 1)
            {
                totalFull += costoTotal;
            }
            else
            {
                totalPrepago += costoTotal;
            }

        }

        private void AñadirAbastecimiento(string nombreCliente)
        {
            abastecimientos.Add(new Abastecimiento(nombreCliente));
        }

        private void AñadirAbastecimientoFull(string nombreCliente)
        {
            abastecimientosFull.Add(new Abastecimiento(nombreCliente));
        }

        private void ActualizarListaAbastecimientos()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = abastecimientos;
        }
        public int ObtenerNumeroAbastecimientosFull()
        {
            return abastecimientosFull.Count;
        }
        private void ActualizarListaAbastecimientosFull()
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = abastecimientosFull;
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
                    label9.Text = $"{costoTotal.ToString("0.00")} Q";
                    label1.Text = contadorLitros.ToString("0.00");
                    double vuelto = cantidadDeseada - costoTotal;
                    label13.Text = $"  {vuelto.ToString("0.0")} Q";

                    if (!EsTanqueLleno())
                    {
                        if (contadorLitros >= cantidadDeseada / PrecioLitro)
                        {
                            contador1++;
                            DetenerTimers();
                            ApagarMotor();
                            MostrarMensajeTanqueLleno();
                        }
                    }
                }
                else
                {
                    DetenerTimers();
                    ApagarMotor();
                    MostrarMensajeTanqueLleno();
                }
            }
        }

        private bool EsTanqueLleno()
        {

            return (TanqueLlenoSuper == 1);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            double incremento = 1.0 / (14000 / INTERVALO_MILISEGUNDOS);
            contadorLitros += incremento;
            label5.Text = contadorLitros.ToString("0.00");

            double costoTotal = contadorLitros * PrecioLitro;
            label10.Text = $"Costo total: {costoTotal.ToString("0.00")} Q";
        }
        public double ObtenerTotalPrepago()
        {
            return totalPrepago;
        }
        public double ObtenerTotalFull()
        {
            return totalFull;
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
            label9.Text = string.Empty;
            label10.Text = string.Empty;
            label13.Text = string.Empty;
            timer1.Stop();
            timer2.Stop();
            button1.Enabled = true;
            button4.Enabled = true;
        }

        private void Super_FormClosing(object sender, FormClosingEventArgs e)
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
            var result = MessageBox.Show("¿Está seguro que quiere cambiar el precio? Al elegir 'Sí', se eliminarán los datos del día y se reiniciará el sistema", "Confirmación", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string nuevoPrecio = Interaction.InputBox("Ingrese el nuevo precio por litro:", "Nuevo Precio por Litro", PrecioLitro.ToString());
                if (double.TryParse(nuevoPrecio, out double precio))
                {
                    PrecioLitro = precio;
                    label8.Text = $"{PrecioLitro} Q por litro";
                    abastecimientos.Clear();
                    abastecimientosFull.Clear();
                    totalFull = 0;
                    totalPrepago = 0;
                    ReiniciarContadores();
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un precio válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void button2_Click_2(object sender, EventArgs e)
        {
            DetenerBomba();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DetenerBomba();
        }


        public void DetenerBomba()
        {
            DetenerTimers();
            ApagarMotor();
            MessageBox.Show("Bomba detenida.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
