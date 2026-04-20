using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Control_SEV.Motion_Card.ControlCycleRun
{
    public partial class frmProcess : Form
    {
       
        public frmProcess()
        {
            InitializeComponent();
            dgvtProcess.AllowUserToAddRows = false;
            DataGridViewTextBoxColumn colText = new DataGridViewTextBoxColumn();
            colText.HeaderText = "NameProcess";
            colText.Name = "colNameProcess";
            DataGridViewComboBoxColumn colCombo = new DataGridViewComboBoxColumn();
            colCombo.HeaderText = "TypeProcess";
            colCombo.Name = "colTypeProcess";
            colCombo.DataSource = new List<string> { "RunProcess", "WaitProcess", "WriteProcess" };
            dgvtProcess.Columns.Add(colText);
            dgvtProcess.Columns.Add(colCombo);
        }

     

        private void btnAdd_Click(object sender, EventArgs e)
        {
          
           int row = dgvtProcess.Rows.Add();
            dgvtProcess.Rows[row].Cells["colNameProcess"].Value = "Process Name"; 


        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            dgvtProcess.Rows.RemoveAt(dgvtProcess.CurrentRow.Index);
        }

        private void dgvtProcess_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox cb = e.Control as ComboBox;

            if (cb != null)
            {

                cb.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                cb.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enum.TryParse((sender as ComboBox).SelectedItem.ToString(),out TypeProcess typeProcess1);
            pnlProcess.Controls.Clear();
            if (typeProcess1 == TypeProcess.RunProcess)
            {
                ucRunProcess _runProcess = new ucRunProcess();
                pnlProcess.Controls.Add(_runProcess);
                
            }
            else if (typeProcess1 == TypeProcess.WriteProcess)
            {
                ucWaitProcess _waitProcess = new ucWaitProcess();
                pnlProcess.Controls.Add(_waitProcess);
            }
            else
            {
                ucWriteProcess _writeProcess = new ucWriteProcess();
                pnlProcess.Controls.Add(_writeProcess);
            }

            

        }
       private enum TypeProcess
        {
            RunProcess,
            WaitProcess,
            WriteProcess
        }
    }
}
