using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace LedControl {
    
    /// <summary>
    /// The serial communication class
    /// </summary>
    internal class SerialCom {

        // Variables
        private SerialPort port;

        private string comport;
        private int baudrate;
        private byte[] syncBytes = { (byte)'A', (byte)'d', (byte)'a' };


        /// <summary>
        /// Property for the COM port
        /// </summary>
        public string Comport{
            get{
                return this.comport;
            }
        }

        /// <summary>
        /// Non-Default Constructor
        /// </summary>
        /// <param name="comport">The COM port</param>
        /// <param name="baudrate">The Baudrate</param>
        public SerialCom(string comport, int baudrate) {
            this.comport = comport;
            this.baudrate = baudrate;
            this.port = new SerialPort(comport, baudrate);
            this.port.Open();
            this.port.NewLine = "\n";
            this.port.DataReceived += new SerialDataReceivedEventHandler(this.comRecieveHandler);
        }


        /// <summary>
        /// Stop the serial connection
        /// </summary>
        public void StopSerial(){
            this.port.Close();
        }

        public void comRecieveHandler(object o, SerialDataReceivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(port.ReadExisting());
        }


        /// <summary>
        /// Function to set arduino in a mode
        /// </summary>
        /// <param name="modus">Mode selection byte</param>
        /// <param name="options">Extra mode-specific options (eg. number of bytes to follow)</param>
        /// <param name="red">The Red color</param>
        /// <param name="green">The green color</param>
        /// <param name="blue">The blue color</param>
        public void Send(byte[] rgbData) { //modes met 1 kleur input
            
            byte[] data = new byte[rgbData.Length+3+3];

            Array.Copy(syncBytes, data, 3);

            byte chk, hi = 250, lo = 5;
            chk = (byte)(hi ^ lo ^ 0x55);
            Array.Copy(new byte[] { hi, lo, chk }, 0, data, 3, 3);
            
            Array.Copy(rgbData, 0, data, 3+3, rgbData.Length);

            port.Write(data, 0, rgbData.Length + 3 + 3);

        }
    }
}
