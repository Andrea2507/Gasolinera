using System;
using System.Windows.Forms;

namespace gasolinera_json
{
    public partial class Registro : Form
    {
        private Super super;

        public Registro(Super super)
        {
            InitializeComponent();
            this.super = super;
        }

        private void Registro_Load(object sender, EventArgs e)
        {
            ActualizarContador(super.ObtenerNumeroAbastecimientos());
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public void ActualizarContador(int contador)
        {
            label3.Text = contador.ToString();
        }
    }
}
