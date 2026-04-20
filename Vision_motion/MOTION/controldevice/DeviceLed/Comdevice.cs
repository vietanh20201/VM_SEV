using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
namespace PC_Control_SEV.ControlDevice.DeviceLed
{
    public  class Comdevice
    {
      
        private SerialPort _serialPort;
        // test git

        public event Action<string> DataReceived;

        public bool IsOpen
        {
            get
            {
                if (_serialPort == null) return false;
                return _serialPort.IsOpen;
            }
        }

       
        public bool Open(string portName, int baudRate, int databit , System.IO.Ports.Parity parity)
        {
            try
            {
                _serialPort = new SerialPort();

                _serialPort.PortName = portName;
                _serialPort.BaudRate = baudRate;
                _serialPort.DataBits = databit;
                _serialPort.Parity = parity;
              
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.DataReceived += SerialPort_DataReceived;

                _serialPort.Open();
               
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string[] ScanPorts()
        {
            return SerialPort.GetPortNames();
        }


        public void Close()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void Send(string data)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.WriteLine(data);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadExisting();

            DataReceived?.Invoke(data);
        }
    }
}
