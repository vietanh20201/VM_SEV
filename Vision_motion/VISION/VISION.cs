using BeevisionSolution.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision_motion.VISION
{
    public partial class VISION : Form
    {
        public VISION()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void VISION_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    elementHost1.Child = VisionEngine.GetUI();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Display Error: " + ex.Message);
            //}
        }

        private void VISION_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (elementHost1.Child != null)
                {
                    elementHost1.Child = null;
                }

            }
            catch { }
            base.OnFormClosing(e);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                labelResult.Text = "Status: Triggering...";
                // Example: Triggering for PCS Index 5 on Sheet "SHT-TEST-001"
                bool ok = await VisionEngine.Trigger("SHT-TEST-001", 5);
                labelResult.Text = "Status: " + (ok ? "FINISHED OK" : "FINISHED NG");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Trigger Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var res = VisionEngine.GetProductResult("SHT-TEST-001", 5);

            if (res != null)
            {
                labelResult.Text = $"Product: {res.ProductId}\nStatus: {res.Status}\nFinal: {(res.IsOK ? "OK" : "NG")}";
            }
            else
            {
                labelResult.Text = "Result: Product not found or not yet processed";
            }
        }
    }
}
