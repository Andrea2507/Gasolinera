using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gasolinera_json
{
    internal class Abastecimiento
    {
        public DateTime Fecha { get; }
        public TimeSpan Hora { get; }
        public string NombreCliente { get; }

        public Abastecimiento(string nombreCliente)
        {
            Fecha = DateTime.Today;
            Hora = DateTime.Now.TimeOfDay;
            NombreCliente = nombreCliente;
        }
    }
}
