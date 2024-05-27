using System;
using System.Windows.Forms;

namespace gasolinera_json
{
    public partial class Form2 : Form
    {
        private Super super;

        public Form2()
        {
            InitializeComponent();
            super = new Super();
            super.FormClosing += Super_FormClosing;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (super.IsDisposed) 
            {
                super = new Super();
                super.FormClosing += Super_FormClosing; // Volvemos a añadir el manejador de eventos si se ha creado una nueva instancia
            }
            super.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Registro registro = new Registro(super);
            registro.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Regular regular = new Regular();
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
            super.Hide();
        }
    }
}
