using DevExpress.Entity.Model.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PC_Control_SEV;

namespace Vision_motion
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            open_form_motion();
        }

        private void Computer_vision(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        private void PC_CONTROL(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string fName = "Main_control";
            var f = GetFormChild(fName);
            if (f == null)
            {
                f = new Main_control();
                f.MdiParent = this;
                f.Name = fName;
                f.Show();
                this.FormClosing += (s, v) =>
                {
                    try
                    {
                        if (f != null)
                        {
                            f.Close();
                        }
                    }
                    catch { }
                };
            }
            else
            {
                f.Activate();
            }
        }
        public void open_form_motion()
        {
            string fName = "Main_control";
            var f = GetFormChild(fName);
            if (f == null)
            {
                f = new Main_control();
                f.MdiParent = this;
                f.Name = fName;
                f.Show();
                this.FormClosing += (s, v) =>
                {
                    try
                    {
                        if (f != null)
                        {
                            f.Close();
                        }
                    }
                    catch { }
                };
            }
            else
            {
                f.Activate();
            }
        }
        
        private Form GetFormChild(string fName)
        {
            return this.MdiChildren.FirstOrDefault(fr => fr.Name == fName);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}
