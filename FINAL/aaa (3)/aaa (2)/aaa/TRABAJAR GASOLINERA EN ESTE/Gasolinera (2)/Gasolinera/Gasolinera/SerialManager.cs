using System;
using System.IO.Ports;

namespace gasolinera_json
{
    public static class SerialManager
    {
        private static SerialPort _arduino;

        public static SerialPort Arduino
        {
            get
            {
                if (_arduino == null)
                {
                    _arduino = new SerialPort("COM3", 9600); 
                }
                return _arduino;
            }
        }

        public static void AbrirPuertoSerial()
        {
            if (_arduino == null)
            {
                _arduino = new SerialPort("COM3", 9600); 
            }

            if (!_arduino.IsOpen)
            {
                _arduino.Open();
            }
        }

        
    }
}
