using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Control_SEV.Motion_Manager.model.config_data_model
{
    public class Data_Router_Led
    {
        [Category("Device")]
        [DisplayName("Device Name")]
        public string DeviceName { get; set; } = "";

        [Category("Port")]
        [DisplayName("Port Name")]
        public string PortName { get; set; } = "COM1";

        [Category("Port")]
        [DisplayName("BaudRate")]
        public int BaudRate { get; set; } = 9600;

        [Category("Port")]
        [DisplayName("DataBits")]
        public int DataBits { get; set; } = 8;

        [Category("Port")]
        [DisplayName("Parity")]
        public Parity Parity { get; set; } = Parity.None;

        [Category("Port")]
        [DisplayName("StopBits")]
        public StopBits StopBits { get; set; } = StopBits.One;
        [Category("Port")]
        [DisplayName("AutoConnected")]
        public bool AutoConnected { get; set; } = false;


    }
    public class CYLINDER
    {
        [Category("CYLINDER")]
        [DisplayName("CYLINDER_NUMBER")]
        public int No { get; } = 1;
        [Category("CYLINDER")]
        [DisplayName("BIT_CYLINDER")]
        public int BIT_CYLINDER { get; set; } = 0;


    }
    public class HOME
    {
        [Category("HOME")]
        [DisplayName("HOME_Method")]
        public int Home_Method { get; set; } = 0;
        [Category("HOME")]
        [DisplayName("OffSet")]
        public int OffSet { get; set; } = 0;
        [Category("HOME")]
        [DisplayName("HighVel")]
        public int HighVel { get; set; } = 0;
        [Category("HOME")]
        [DisplayName("LowVel")]
        public int LowVel { get; set; } = 0;
        [Category("HOME")]
        [DisplayName("ACC_DECC")]
        public int ACC_DECC { get; set; } = 0;


    }
    public class LocalFileLog
    {
        public string LogDirectory { get; set; } = "Application.StartupPath";
      
    }
}
