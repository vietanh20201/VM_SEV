namespace Vision_motion.MOTION.motion_manager.Login
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtNewPassWord = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtPassWord = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNewPassWord = new Guna.UI2.WinForms.Guna2Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.Controls.Add(this.txtNewPassWord, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPassWord, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNewPassWord, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.54369F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.45631F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(677, 103);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // txtNewPassWord
            // 
            this.txtNewPassWord.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNewPassWord.DefaultText = "";
            this.txtNewPassWord.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNewPassWord.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNewPassWord.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPassWord.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNewPassWord.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPassWord.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNewPassWord.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPassWord.Location = new System.Drawing.Point(239, 54);
            this.txtNewPassWord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNewPassWord.Name = "txtNewPassWord";
            this.txtNewPassWord.PlaceholderText = "New Pass Word";
            this.txtNewPassWord.SelectedText = "";
            this.txtNewPassWord.Size = new System.Drawing.Size(435, 45);
            this.txtNewPassWord.TabIndex = 3;
            this.txtNewPassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPassWord
            // 
            this.txtPassWord.Animated = true;
            this.txtPassWord.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPassWord.DefaultText = "";
            this.txtPassWord.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPassWord.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPassWord.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassWord.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassWord.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPassWord.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPassWord.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPassWord.Location = new System.Drawing.Point(239, 4);
            this.txtPassWord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PlaceholderText = "Pass Word";
            this.txtPassWord.SelectedText = "";
            this.txtPassWord.Size = new System.Drawing.Size(435, 42);
            this.txtPassWord.TabIndex = 0;
            this.txtPassWord.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassWord.UseSystemPasswordChar = true;
            this.txtPassWord.Enter += new System.EventHandler(this.txtPassWord_Enter);
            this.txtPassWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassWord_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 50);
            this.label1.TabIndex = 4;
            this.label1.Text = "Pass Word";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNewPassWord
            // 
            this.btnNewPassWord.BorderRadius = 3;
            this.btnNewPassWord.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNewPassWord.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnNewPassWord.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnNewPassWord.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnNewPassWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewPassWord.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnNewPassWord.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNewPassWord.ForeColor = System.Drawing.Color.Black;
            this.btnNewPassWord.Location = new System.Drawing.Point(3, 53);
            this.btnNewPassWord.Name = "btnNewPassWord";
            this.btnNewPassWord.Size = new System.Drawing.Size(230, 47);
            this.btnNewPassWord.TabIndex = 5;
            this.btnNewPassWord.Text = "New Pass Word";
            this.btnNewPassWord.Click += new System.EventHandler(this.btnNewPassWord_Click);
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 103);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2TextBox txtPassWord;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPassWord;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnNewPassWord;
    }
}