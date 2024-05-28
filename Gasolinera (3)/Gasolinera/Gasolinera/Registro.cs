using System;
using System.Windows.Forms;

namespace gasolinera_json
{
    public partial class Registro : Form
    {
        private Super super;
        private Regular regular;

        public Registro(Super super, Regular regular)
        {
            InitializeComponent();
            this.super = super;
            this.regular = regular;
        }

        private void Registro_Load(object sender, EventArgs e)
        {
            ActualizarContador(super.ObtenerNumeroAbastecimientos(), regular.ObtenerNumeroAbastecimientos());
     
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public void ActualizarContador(int contador, int contador2)
        {
            label3.Text = contador.ToString();
            label9.Text = contador2.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label3.Text == label9.Text)
            {
                MessageBox.Show("Las bombas han sido igual de utilizadas");
            }
            if (Int32.Parse(label3.Text) > Int32.Parse(label9.Text))
            {
                MessageBox.Show("La bomba super ha sido la mas utilizada");
            }
            if (Int32.Parse(label3.Text) < Int32.Parse(label9.Text))
            {
                MessageBox.Show("La bomba regular ha sido la mas utilizada");
            }
        }
    }
}
