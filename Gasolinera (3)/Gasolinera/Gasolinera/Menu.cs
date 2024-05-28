using System;
using System.Windows.Forms;

namespace gasolinera_json
{
    public partial class Form2 : Form
    {
        private Super super;
        private Regular regular;

        public Form2()
        {
            InitializeComponent();
            super = new Super();
            super.FormClosing += Super_FormClosing;
            regular = new Regular();
            regular.FormClosing += Regular_FormClosing;
        }

        private void label1_Click(object sender, EventArgs e)
        {

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
            Registro registro = new Registro(super, regular);
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
            super.Hide();
        }
        private void Regular_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            regular.Hide();
        }
    }
}
