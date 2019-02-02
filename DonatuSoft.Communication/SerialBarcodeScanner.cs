using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonatuSoft.Communication
{
    public class SerialBarcodeScanner
    {
        SerialPort com;
        string port;
        int baudRate;
        Parity parity;
        int dataBits;
        StopBits stopBits;

        public SerialBarcodeScanner(string port, int baudRate, string parity, int dataBits, string stopBits)
        {           
            switch (parity)
            {
                case "o":
                    this.parity = Parity.Odd;
                    break;
                case "e":
                    this.parity = Parity.Even;
                    break;
                case "m":
                    this.parity = Parity.Mark;
                    break;
                case "s":
                    this.parity = Parity.Space;
                    break;
                default:
                    this.parity = Parity.None;
                    break;
            }

            this.dataBits = dataBits;

            switch (stopBits)
            {
                case "1.5":
                    this.stopBits = StopBits.OnePointFive;
                    break;
                case "2":
                    this.stopBits = StopBits.Two;
                    break;
                default:
                    this.stopBits = StopBits.One;
                    break;
            }

            com = new SerialPort(this.port, this.baudRate, this.parity, this.dataBits, this.stopBits);
        }

        public bool Connect()
        {
            if (!com.IsOpen)
            {
                com.Open();
                return true;
            }
            else
                return false;
        }

        public void Disconnect()
        {
            com.Close();
        }

        void com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Initialize a buffer to hold the received data 
            byte[] buffer = new byte[com.ReadBufferSize];

            //There is no accurate method for checking how many bytes are read 
            //unless you check the return from the Read method 
            int bytesRead = com.Read(buffer, 0, buffer.Length);

            //For the example assume the data we are received is ASCII data. 
            tString += Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //Check if string contains the terminator  
            if (tString.IndexOf((char)_terminator) > -1)
            {
                //If tString does contain terminator we cannot assume that it is the last character received 
                string workingString = tString.Substring(0, tString.IndexOf((char)_terminator));
                //Remove the data up to the terminator from tString 
                tString = tString.Substring(tString.IndexOf((char)_terminator));
                //Do something with workingString 
                Console.WriteLine(workingString);
            }
        }
    }
}
