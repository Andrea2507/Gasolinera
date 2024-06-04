using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace gasolinera_json
{
    public partial class Form2 : Form
    {
        private Super super;
        private Regular regular;
        private Diesel diesel;
        private ION_Diesel id;
        private SerialPort arduino;

        public Form2()
        {
            InitializeComponent();
            arduino = new SerialPort("COM3", 9600);
            arduino.DataReceived += Arduino_DataReceived;

            super = new Super();
            regular = new Regular();

            diesel = new Diesel();
            id = new ION_Diesel();

            super.FormClosing += Super_FormClosing;
            regular.FormClosing += Regular_FormClosing;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                if (!arduino.IsOpen)
                {
                    arduino.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el puerto: " + ex.Message);
            }
        }

        private void Arduino_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = arduino.ReadLine();
            this.Invoke(new Action(() => MessageBox.Show("Data received: " + data)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (super.IsDisposed)
            {
                super = new Super();
                super.FormClosing += Super_FormClosing;
            }
            super.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Registro registro = new Registro(super, regular, diesel, id);
            registro.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (regular.IsDisposed)
            {
                regular = new Regular();
                regular.FormClosing += Regular_FormClosing;
            }
            regular.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Diesel diesel = new Diesel();
            diesel.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ION_Diesel ion = new ION_Diesel();
            ion.Show();
        }

        private void Super_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

        }

        private void Regular_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
           
        }
        private void Diesel_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

        }
        private void ION_Diesel_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (arduino.IsOpen)
            {
                arduino.Close();
            }
        }
        

    }
}
