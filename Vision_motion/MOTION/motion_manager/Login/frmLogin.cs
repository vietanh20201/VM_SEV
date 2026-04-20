using QC_Vision.Manager_path;
using QC_Vision.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Vision_motion.MOTION.motion_manager.Login
{
    public partial class frmLogin : Form
    {
        public Action _PassWordCorrect;
        private string path_manager = Manager_path.path_manager;
        private string path_Login = Manager_path.path_manager + "\\LoginPassWord.json";
        public frmLogin()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNewPassWord_Click(object sender, EventArgs e)
        {
            if (File.Exists(path_Login))
            {
                Config_Login._Login = Config_Login.LoadFromJson(path_Login);
                if (txtNewPassWord.Text.Length != 0 && Config_Login._Login.Pass_Word == txtPassWord.Text)
                {
                    Manager_path.Creat_path(path_manager);
                    Config_Login._Login.Pass_Word = txtNewPassWord.Text;
                    Config_Login.SaveToJson(path_Login);
                }
                else
                {
                    MessageBox.Show("Hãy Nhập PassWord hoặc Nhập Đúng Mật Khẩu Cũ");
                }
            }
            else
            {

             if (txtNewPassWord.Text.Length != 0)
                {
                    Manager_path.Creat_path(path_manager);
                    Config_Login._Login.Pass_Word = txtNewPassWord.Text;
                    Config_Login.SaveToJson(path_Login);
                }
                else
                {
                    MessageBox.Show("Hãy Nhập PassWord hoặc Nhập Đúng Mật Khẩu Cũ");
                }
            }
        }

        private void txtPassWord_Enter(object sender, EventArgs e)
        {
           
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();    
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
        
            if (e.KeyCode == Keys.Enter)
            {
                if (File.Exists(path_Login))
                {
                    Config_Login._Login = Config_Login.LoadFromJson(path_Login);
                    if (Config_Login._Login.Pass_Word == txtPassWord.Text)
                    {
                        _PassWordCorrect?.Invoke();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Pass Word is not Correct");
                    }
                }
                else
                {
                    MessageBox.Show("Chưa tạo PassWord");
                }
              
            }
        }
    
    }
}
