using System;

namespace gasolinera_json
{
    internal class Abastecimiento
    {
        public DateTime Fecha { get; }
        public TimeSpan Hora { get; }
        public string NombreCliente { get; }

        public int cantidad { get; set; }

        public Abastecimiento(string nombreCliente)
        {
            Fecha = DateTime.Today;
            Hora = DateTime.Now.TimeOfDay;
            NombreCliente = nombreCliente;
        }
        public Abastecimiento()
        {
            cantidad = cantidad;
        }
    }
}
