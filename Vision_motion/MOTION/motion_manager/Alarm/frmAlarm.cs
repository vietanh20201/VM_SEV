using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Control_SEV.Motion_Manager.Alarm
{
    public partial class frmAlarm : Form
    {
        public frmAlarm()
        {
            InitializeComponent();
        }
        public void Alarm(string alarm)
        {
            tbAlarm.Text = "Error :" + alarm;
        }
    }
}
