

using BeevisionSolution.Controller;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using PC_Control_SEV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vision_motion.VISION;

namespace Bonding_Vision
{
    public partial class Form_Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static Form_Main instance;
        public Form_Main()
        {
            InitializeComponent();
            //UserLookAndFeel.Default.SetSkinStyle("DevExpress Dark Style");
            instance = this;
            show_home();
            Control.CheckForIllegalCrossThreadCalls = false;
            ribbonControl1.Minimized = true;
        }
        private void Select_Model(object sender,
            ItemClickEventArgs e)
        {
        }
        private Form GetFormChild(string fName)
        {
            return this.MdiChildren.FirstOrDefault(fr => fr.Name == fName);
        }
        private void Setting_click(object sender,
            ItemClickEventArgs e)
        {
            string fName = "Main_control";
            var f = GetFormChild(fName);
            if (f == null)
            {
                f = new Main_control();
                f.MdiParent = this;
                f.Name = fName;
                f.Show();
            }
            else
            {
                f.Activate();
            }
        }

        private void Camera_live_click(object sender,
            ItemClickEventArgs e)
        {
        }
        private void Save_config_teaching(object sender,
            ItemClickEventArgs e)
        {
        }
        private async void Form_Main_Load(object sender, EventArgs e)
        {
            try
            {
                await VisionEngine.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vision Init Error: " + ex.Message);
            }
        }
        private void Home(object sender, ItemClickEventArgs e)
        {
            string fName = "VISION";
            var f = GetFormChild(fName);
            if (f == null)
            {
                f = new VISION();
                f.MdiParent = this;
                f.Name = fName;
                f.Show();
            }
            else
            {
                f.Activate();
            }
        }
        private void Form_Main_FormClosing(object sender,
            FormClosingEventArgs e)
        {

            try
            {
                VisionEngine.Shutdown();
            }
            catch { }
            string processName = "Development_vision_ver2";
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to terminate process {process.ProcessName} with ID {process.Id}: {ex.Message}");
                }
            }
            //AppPro.Exit = true;
            this.Close();
            Application.Exit();

        }
        public void show_home()
        {
        }
        private void Show_Communication(object sender,
            ItemClickEventArgs e)
        {
        }
        private void Check_connect_plc_timer
            (object sender, EventArgs e)
        {
        }
        private void Show_MX_Component
            (object sender, ItemClickEventArgs e)
        {
        }
        private void Show_Modbus_TCP_IP
            (object sender, ItemClickEventArgs e)
        {
        }
        private void Show_Auto_Calib_9_points
            (object sender, ItemClickEventArgs e)
        {
        }        private void barToggleSwitchItem1_CheckedChanged
            (object sender, ItemClickEventArgs e)
        {
           
        }//Py pâss
        private void barButtonItem9_ItemClick
            (object sender, ItemClickEventArgs e)
        {
           
        }
        private void barButtonItem12_ItemClick
            (object sender, ItemClickEventArgs e)
        {
        }
        private void Indicator_grid_show(object sender, ItemClickEventArgs e)
        {
        }
       

    }
}
