using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;
using PC_Control_SEV.ControlDevice.DeviceLed;
using PC_Control_SEV.Motion_Card;
using PC_Control_SEV.Motion_Manager.Alarm;
using PC_Control_SEV.Motion_Manager.mode;
using PC_Control_SEV.Motion_Manager.model.config_data_model;
using PC_Control_SEV.Motion_Manager.thread;
using QC_Vision.Config_data_model;
using QC_Vision.Manager_path;
using QC_Vision.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PC_Control_SEV.Motion_Card.ControlCycleRun;
using Guna.UI2.WinForms;
using DevExpress.XtraPrinting.BarCode.Native;
using Utilities.LogProgram;
using Vision_motion.MOTION.motion_manager.Login;

namespace PC_Control_SEV
{
    public partial class Main_control : System.Windows.Forms.Form
    {
        private frmLogin _Login;
        private bool flag_Led =true;
        private int Current_Value_Led;
        private readonly object _Lock_Led = new object();
        public static Action<string> _Alarm;
        private Comdevice Router, Led;
        private frmAlarm _frmAlarm;
        private System.Threading.Timer timerAxis;
        double[] nTimerAxPrfPos = { 0 };
        Int32[] nTimerAxSts = { 0 };
        Int16[] nTimerAxPrfMode = { 0 };
        double[] nTimerAxPrfVel = { 0 };
        private System.Windows.Forms.Timer axStsMonitor_timer;
        Int16 nTempAxNo = 0;
        UInt32 ret = 0;
        Int32 nCardNo = 0;
        UInt64 nCardHandle = 0;
        Int16 nCrdNo = 0;
        Int32 nTimerAxInput; 
        double[] nTimerAxPrfAcc = { 0 };
        double[] nTimerAxEncPos = { 0 };
        double[] nTimerAxEncVel = { 0 };
        double[] nTimerAxEncAcc = { 0 };
        string model_select = "";
        int selected_row_index = -1;
        private bool tabPage_setting_Enabled = true;
        private DateTimeLabelThread _clock;
        private string path_manager = Manager_path.path_manager;
        private string path_model = Manager_path.path_manager +"\\model.json";
        private string path_LocalFile = Manager_path.path_manager + "\\LocalFile.json";
        public static string path_LocalFileLog;

        private Data_Router_Led serialRouter, serialLed;
        private CYLINDER _CYLINDER;
        private HOME _HOME;
        public Main_control()
        {
            InitializeComponent();
            
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _frmAlarm = new frmAlarm();
            SetupTabStyle();
            SetupTabIcons();
            //setup_button(Load_model, Properties.Resources.update, ContentAlignment.MiddleLeft);
            //setup_button(Delete_model, Properties.Resources.bin, ContentAlignment.MiddleLeft);
            //setup_button(Create_model, Properties.Resources.add_file, ContentAlignment.MiddleLeft);
            //setup_button(Jog_positive, Properties.Resources.previous, ContentAlignment.MiddleLeft);
            //setup_button(Jog_negative, Properties.Resources.next, ContentAlignment.MiddleRight);
            //setup_button(Move_pos, Properties.Resources.arrows, ContentAlignment.MiddleLeft);
            //setup_button(Save_pos, Properties.Resources.save, ContentAlignment.MiddleLeft);
            //setup_button(Teach_pos, Properties.Resources.selective, ContentAlignment.MiddleLeft);
            //setup_button(Remove_pos, Properties.Resources.eraser, ContentAlignment.MiddleLeft);
            //setup_button(Add_pos, Properties.Resources.plus, ContentAlignment.MiddleLeft);
            //setup_button(Clear_pos, Properties.Resources.bin, ContentAlignment.MiddleLeft);
            //setup_button(Auto_mode, Properties.Resources.Auto, ContentAlignment.MiddleLeft);
            //setup_button(Manual_mode, Properties.Resources.Manual, ContentAlignment.MiddleLeft);
            //setup_button(Origin_mode, Properties.Resources.Origin1, ContentAlignment.MiddleLeft);
            //setup_button(Reset_mode, Properties.Resources.reset, ContentAlignment.MiddleLeft);
            //setup_button(Insert_pos, Properties.Resources.insert, ContentAlignment.MiddleLeft);
            _Login=new frmLogin();
            Load_File_Local();
            Load_model_config();
            InitDevice();
            Update_grid();
            
            Managa_pos.RowPostPaint += dataGridView1_RowPostPaint;
            Managa_pos.CellClick += dataGridView1_CellClick;
            tabControl1.Selecting += tabControl1_Selecting;
            setup_status_mode();
            _clock = new DateTimeLabelThread(Date_time, 200);
            _clock.Start("yyyy-MM-dd" + "\n" + " HH:mm:ss"); // hoặc "yyyy-MM-dd HH:mm:ss"
            _Alarm = (message) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (lbAlarm.Items.Count > 500)
                    {
                        lbAlarm.Items.RemoveAt(0);
                    }
                   
                    lbAlarm.Items.Add($"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss:fff")} : {message}");
                    LogProgram.MesWriteLog($"{message}");
                }));
            };
            Setting_Control.Instance.Logs = (s) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (lbLog.Items.Count > 500)
                    {
                        lbLog.Items.RemoveAt(0);
                    }
                    lbLog.Items.Add($"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss:fff")} : {s}");
                    LogProgram.WriteLog($"{s}");
                }));
            };
            if (Setting_Control.Instance.InitCard())
            {
                Setting_Control.Instance.StartHome(short.Parse(txtHome.Text), int.Parse(txtOffSet.Text), double.Parse(txtHighVel.Text) * 100, double.Parse(txtLowVel.Text) * 100, double.Parse(txtAccDec.Text));
                btnServo.Enabled = true;
            }
            else
            {
                btnServo.Enabled = false;
            }


            Setting_Control.Instance.AxisStatusChanged += UpdateAxisUI;
            timerAxis = new System.Threading.Timer(TimerCallback, null, 0, 200);
            Current_Value_Led = guna2VTrackBar1.Value;
            propertyPosition.PropertySort = PropertySort.Categorized;




        }
        private void TimerCallback(object state)
        {
            try
            {
                Setting_Control.Instance.MonitorAxis();
            }
            catch (Exception ex)
            {
                Main_control._Alarm?.Invoke("MonitorAxis crash: " + ex.Message);
            }
        }
        private void UpdateAxisUI(string name, object value)
        {
            Control ctrl = this.Controls.Find(name, true)[0];

            if (ctrl == null) return;

            if (ctrl is Label lbl)
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        if (value is bool)
                            lbl.BackColor = (bool)value ? Color.Red : Color.LightGreen;
                        else
                            lbl.Text = value.ToString();
                    }));
                }
            }
        }


        private void SetupTabStyle()
        {
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed; // BẮT BUỘC nếu dùng DrawItem
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(200, 50);
            tabControl1.Padding = new Point(0, 1);

            tabControl1.DrawItem -= tabControl1_DrawItem;
            tabControl1.DrawItem += tabControl1_DrawItem;
        }
        private void SetupTabIcons()
        {
            var imgList = new ImageList
            {
                ImageSize = new Size(52, 52),          // icon nhỏ vừa tab
                ColorDepth = ColorDepth.Depth32Bit
            };

            //imgList.Images.Add(Properties.Resources.house);
            //imgList.Images.Add(Properties.Resources.settings);
            //imgList.Images.Add(Properties.Resources.data_modeling);
            //imgList.Images.Add(Properties.Resources.alarm);

            tabControl1.ImageList = imgList;

            Main.ImageIndex = 0;
            Setting.ImageIndex = 1;
            Model.ImageIndex = 2;
            Alarm.ImageIndex = 3;
        }
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tc = (TabControl)sender;
            TabPage page = tc.TabPages[e.Index];
            Rectangle r = tc.GetTabRect(e.Index);
            bool selected = (e.Index == tc.SelectedIndex);

            // Flat colors
            Color normalBack = Color.White;
            Color selectedBack = Color.FromArgb(200, 230, 220);
            Color underline = Color.FromArgb(60, 160, 120);
            Color textColor = Color.FromArgb(70, 70, 70);

            // Fill background
            using (var b = new SolidBrush(selected ? selectedBack : normalBack))
                e.Graphics.FillRectangle(b, r);

            // --- ICON (sát trái) ---
            int padLeft = 6;               // sát trái hơn thì giảm nữa (0~6)
            int iconSize = 24;

            Image img = null;
            if (tc.ImageList != null && page.ImageIndex >= 0 && page.ImageIndex < tc.ImageList.Images.Count)
                img = tc.ImageList.Images[page.ImageIndex];

            int iconX = r.Left + padLeft;
            int iconY = r.Top + (r.Height - iconSize) / 2;

            if (img != null)
                e.Graphics.DrawImage(img, new Rectangle(iconX, iconY, iconSize, iconSize));

            // --- TEXT (bên phải icon) ---
            int textLeft = iconX + iconSize + 8;
            Rectangle textRect = new Rectangle(textLeft, r.Top, r.Width - (textLeft - r.Left), r.Height);

            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                tc.Font,
                textRect,
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );

            // Underline tab selected
            if (selected)
            {
                using (Pen p = new Pen(underline, 3))
                    e.Graphics.DrawLine(p, r.Left + 10, r.Bottom - 2, r.Right - 10, r.Bottom - 2);
            }
        }
        private void setup_button(System.Windows.Forms.Button button, Image image, Enum Image_align)
        {
            // Resize icon 24x24
            Image original = image;
            Bitmap resized = new Bitmap(24,24);

            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, 0, 0, 24, 24);
            }

            button.Image = resized;
            //button1.Text = "Settings";

            // Icon bên trái
            button.ImageAlign = (ContentAlignment)Image_align;

            // Text nằm giữa toàn bộ button
            button.TextAlign = ContentAlignment.MiddleCenter;

            // Quan trọng: Overlay thay vì ImageBeforeText
            button.TextImageRelation = TextImageRelation.Overlay;

            // Đẩy icon sát trái
            button.Padding = new Padding(8, 0, 0, 0);

            button.Height = 40;
        }
        /// <summary>
        /// ///////////////////////////////////////////////// TẠO MODEL MỚI //////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Creat_Model(object sender, EventArgs e)
        {
         
            Manager_path.Creat_path(path_manager);
            Config_Model.model.List_model.Add(Name_model.Text);
            Config_Model.SaveToJson(path_model);
            path_data_model_load(Name_model.Text, out string path_data_model);
            Config_Motion.SaveToJson(path_data_model);
            Load_model_config();
        }
        public void path_data_model_load(string Name_Model, out string path_data_model)
        {
           
            string path_data_model_folder = path_manager + "\\" + Name_Model;
            Manager_path.Creat_path(path_data_model_folder);
            path_data_model = path_data_model_folder + "\\" + Name_Model + ".json";
        }
        private void Load_model_config()
        {
           
            Manager_path.Creat_path(path_manager);
            if (File.Exists(path_model))
            {
                List_model.Items.Clear();
                Config_Model.model = Config_Model.LoadFromJson(path_model);
                foreach (string model_element in Config_Model.model.List_model)
                {
                    List_model.Items.Add(model_element);
                }
            }
            string path_data_model_folder = path_manager + "\\" + Config_Model.model.Current_model;
            string path_data_model = path_data_model_folder + "\\" + Config_Model.model.Current_model + ".json";
            if (File.Exists(path_data_model))
            {
                Config_Motion.prameter =
                    Config_Motion.LoadFromJson(path_data_model);
            }
            Model_curr.Text = Config_Model.model.Current_model;
            Current_Model.Text = "Current_Model: " + Config_Model.model.Current_model;
            
        }
        private void InitDevice()
        {
            #region Load Data Device
            serialRouter = Config_Motion.prameter.Serial_configs.FirstOrDefault(x => x.DeviceName == "Router");
            serialLed = Config_Motion.prameter.Serial_configs.FirstOrDefault(x => x.DeviceName == "Led");
            _CYLINDER = Config_Motion.prameter._cylinder.FirstOrDefault(x => x.No == 1);
            _HOME = Config_Motion.prameter._Home.FirstOrDefault();
            if (serialRouter == null)
            {
                serialRouter = new Data_Router_Led();
                serialRouter.DeviceName = "Router";
                Config_Motion.prameter.Serial_configs.Add(serialRouter);
            }
            if (serialLed == null)
            {
                serialLed = new Data_Router_Led();
                serialLed.DeviceName = "Led";
                Config_Motion.prameter.Serial_configs.Add(serialLed);
            }
            if (_CYLINDER == null)
            {
                _CYLINDER = new CYLINDER();
                _CYLINDER.BIT_CYLINDER = 0;
                Config_Motion.prameter._cylinder.Add(_CYLINDER);
            }
            if (_HOME == null)
            {
                _HOME = new HOME();
                _HOME.Home_Method = 6;
                _HOME.ACC_DECC = 10000;
                _HOME.HighVel = 500;
                _HOME.LowVel = 200;

                Config_Motion.prameter._Home.Add(_HOME);
            }
            //--- Scan Com
            foreach (var com in Comdevice.ScanPorts())
            {
                cbxComRouter.Items.Add(com);
                cbxComLed.Items.Add(com);
            }
            //----
            Router = new Comdevice();
            cbxComRouter.Text = serialRouter.PortName;
            cbxBaudRateRouter.Text = serialRouter.BaudRate.ToString();
            cbxDataBitsRouter.Text = serialRouter.DataBits.ToString();
            cbxParityRouter.Text = serialRouter.Parity.ToString();
            cbxStopBitRouter.Text = serialRouter.StopBits.ToString();
            cbAutoConnectedRouter.Checked = serialRouter.AutoConnected;
            if (serialRouter.AutoConnected && serialRouter.PortName != string.Empty)
            {

                if (!Router.Open(serialRouter.PortName, serialRouter.BaudRate, serialRouter.DataBits, serialRouter.Parity))
                {
                    if (lbLog.Items.Count > 500)
                    {
                        lbLog.Items.RemoveAt(0);
                    }
                    lbLog.Items.Add($"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss:fff")} : Kết Nối Router Không Thành Công");
                    LogProgram.MesWriteLog($"Kết Nối Router Không Thành Công");
                }
            }
            //---
            Led = new Comdevice();
            cbxComLed.Text = serialLed.PortName;
            cbxBaudRateLed.Text = serialLed.BaudRate.ToString();
            cbxDataBitsLed.Text = serialLed.DataBits.ToString();
            cbxParityLed.Text = serialLed.Parity.ToString();
            cbxStopBitLed.Text = serialLed.StopBits.ToString();
            cbAutoConnectedLed.Checked = serialLed.AutoConnected;
            if (serialLed.AutoConnected && serialLed.PortName != string.Empty)
            {

                if (!Led.Open(serialLed.PortName, serialLed.BaudRate, serialLed.DataBits, serialLed.Parity))
                {
                    if (lbLog.Items.Count > 500)
                    {
                        lbLog.Items.RemoveAt(0);
                    }
                    lbLog.Items.Add($"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss:fff")} : Kết Nối Led Không Thành Công");
                    LogProgram.MesWriteLog($"Kết Nối Led Không Thành Công");
                }
            }

            textBox19.Text = _CYLINDER.BIT_CYLINDER.ToString();
            txtHome.Text = _HOME.Home_Method.ToString();
            txtAccDec.Text = _HOME.ACC_DECC.ToString();
            txtHighVel.Text = _HOME.HighVel.ToString();
            txtLowVel.Text = _HOME.LowVel.ToString();
            txtOffSet.Text = _HOME.OffSet.ToString();

            #endregion
        }

        private void Load_model_DB(object sender, EventArgs e)
        {
            string path_manager = Manager_path.path_manager;
            string path_model = path_manager + "\\model.json";
            Config_Model.model.Current_model = model_select;
            Config_Model.SaveToJson(path_model);
            path_data_model_load(model_select, out string path_data_model);
            if (File.Exists(path_data_model))
            {
                Config_Motion.prameter =
                    Config_Motion.LoadFromJson(path_data_model);
            }
            Current_Model.Text = "Current_Model: " + Config_Model.model.Current_model;
            Model_curr.Text = Config_Model.model.Current_model;
            Update_grid();
        }
        private void Load_File_Local()
        {
            if (File.Exists(path_LocalFile))
            {
                Config_FileLog._FileLocal = Config_FileLog.LoadFromJson(path_LocalFile);

                path_LocalFileLog = txtLocalFile.Text = Config_FileLog._FileLocal.LogDirectory;
            }


        }
        private void Select_model(object sender, EventArgs e)
        {
            model_select = List_model.SelectedItem.ToString();
            Config_Model.model.Model_select = model_select;
        }
        /// <summary>
        /// /////////////////////////////////////////////////QUẢN LÝ VỊ TRÍ //////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_position_click(object sender, EventArgs e)
        {
            Data_motion Data_motion = new Data_motion();
            Config_Motion.prameter.Data_motion_config.Add(Data_motion);
            Update_grid();
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {


                string rowNumber = (e.RowIndex + 1).ToString();

                var centerFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                Rectangle headerBounds = new Rectangle(
                    e.RowBounds.Left,
                    e.RowBounds.Top,
                    Managa_pos.RowHeadersWidth,
                    e.RowBounds.Height);

                e.Graphics.DrawString(rowNumber,
                    this.Font,
                    SystemBrushes.ControlText,
                    headerBounds,
                    centerFormat);
            }
            catch
            {
                MessageBox.Show("Please Save Model befor check Parameter");
            }

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                if (e.RowIndex < 0) { return; }
                selected_row_index = e.RowIndex;
                Data_motion Data_motion = Config_Motion.prameter.Data_motion_config[selected_row_index];
                if (Data_motion != null)
                {

                    propertyPosition.SelectedObject = Data_motion;

                }
            }
            catch
            {
                MessageBox.Show("Please Save Model befor check Parameter");
            }
        }
        private void Remove_pos_click(object sender, EventArgs e)
        {
            if (selected_row_index==-1||
                selected_row_index>Config_Motion.prameter.Data_motion_config.Count)
            { 
                MessageBox.Show("Please select a position to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            Config_Motion.prameter.Data_motion_config.RemoveAt(selected_row_index);
            Update_grid();
        }
        private void Insert_pos_click(object sender, EventArgs e)
        {
            if (selected_row_index == -1 ||
                selected_row_index > Config_Motion.prameter.Data_motion_config.Count)
            {
                MessageBox.Show("Please select a position to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Data_motion Data_motion = new Data_motion();
            Config_Motion.prameter.Data_motion_config.Insert(selected_row_index, Data_motion);
            Update_grid();
        }
        private void Clear_pos_click(object sender, EventArgs e)
        {
            Config_Motion.prameter.Data_motion_config.Clear();
            Update_grid();
            selected_row_index = -1;
        }
        private void Save_pos_click(object sender, EventArgs e)
        {

            path_data_model_load(Config_Model.model.Current_model, out string path_data_model);
            Config_Motion.SaveToJson(path_data_model);
            Load_model_config();
        }
        private void Update_grid()
        {
            Managa_pos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            Managa_pos.DataSource = null;
            Managa_pos.DataSource = Config_Motion.prameter.Data_motion_config;
            Managa_pos.Columns["Name_Position"].DisplayIndex = 0;

        }
        /// <summary>
        /// //////////////////////////////////////////////QUẢN LÝ MODE //////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auto_mode_click(object sender, EventArgs e)
        {
            if(Model_curr.Text==""|| Model_curr.Text == null)
            {
                _frmAlarm.Alarm("Please Select Model");
                return;
            }
            Mode_Status.Auto_Manual = true;
            setup_status_mode();
            tabPage_setting_Enabled = false;
        }
        private void Manual_mode_click(object sender, EventArgs e)
        {
            _Login.Show();
            _Login._PassWordCorrect += () =>
            {
                Mode_Status.Auto_Manual = false;
                setup_status_mode();
                tabPage_setting_Enabled = true;
            };
          
        }
        private void setup_status_mode()
        {
            if (Mode_Status.Auto_Manual)
            {
                Auto_mode.BackColor = Color.FromArgb(60, 160, 120);
                Manual_mode.BackColor = Color.FromArgb(240, 240, 240);
                Origin_mode.Enabled = false;
            }
            else
            {

                Auto_mode.BackColor = Color.FromArgb(240, 240, 240);
                Manual_mode.BackColor = Color.FromArgb(60, 160, 120);
                Origin_mode.Enabled = true;
            }
        }
        
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == Setting && !tabPage_setting_Enabled)
            {
                e.Cancel = true; // chặn chọn tab
            }
            if (e.TabPage == Model && !tabPage_setting_Enabled)
            {
                e.Cancel = true; // chặn chọn tab
            }
        }
        /// <summary>
        /// //////////////////////////////////////////////QUẢN LÝ Form //////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_control_FormClosing(object sender, FormClosingEventArgs e)
        {
            Router?.Close();
            Led?.Close();
            timerAxis?.Dispose();
            Setting_Control.Instance.Close_Card();
            _clock?.Stop();
            //_clock?.Dispose();
            //base.OnFormClosing(e);
            string processName = "PC_Control_SEV";
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                    ret = ImcApi.IMC_CloseCard(nCardHandle);
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
       
        private void Origin_mode_Click(object sender, EventArgs e)
        {
            Setting_Control.Instance.StartHome(short.Parse(txtHome.Text), int.Parse(txtOffSet.Text),double.Parse(txtHighVel.Text)*100,double.Parse(txtLowVel.Text)*100,double.Parse(txtAccDec.Text));
        }

    

      

        private void btnEMG_Click(object sender, EventArgs e)
        {
            Setting_Control.Instance.EmergencyStop();
        }

        private void Jog_positive_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Setting_Control.Instance.JogPlusUp();
            }
            catch (Exception ex)
            {
                _Alarm?.Invoke("Jog + Up lỗi: " + ex.Message);
            }
        }

        private void Jog_positive_MouseDown(object sender, MouseEventArgs e)
        {

            try
            {
                Setting_Control.Instance.JogPlusDown((double)ScaleValue);
            }
            catch (Exception ex)
            {
                _Alarm?.Invoke("Jog + Down lỗi: " + ex.Message);
            }
        }

        private void Jog_negative_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Setting_Control.Instance.JogMinusDown((double)ScaleValue);
            }
            catch (Exception ex)
            {
                _Alarm?.Invoke("Jog - Down lỗi: " + ex.Message);
            }

        }

        private void Jog_negative_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Setting_Control.Instance.JogMinusUp();
            }
            catch (Exception ex)
            {
                _Alarm?.Invoke("Jog - Up lỗi: " + ex.Message);
            }
        }

        private void Move_pos_Click(object sender, EventArgs e)
        {
            Setting_Control.Instance.StartMoveAbs(double.Parse(ScaleVelPos.ToString()), double.Parse(txtAcc.Text), double.Parse(txtDec.Text), double.Parse(ScalePos.ToString()), (Int16)comboBox1.SelectedIndex);
        }

        private void Move_Stop_Click(object sender, EventArgs e)
        {
            Setting_Control.Instance.Stop();
        }

        private void lblPos_TextChanged(object sender, EventArgs e)
        {
            lblPos2.Text = numTargetPos.Text;
        }

        private void lblVel_TextChanged(object sender, EventArgs e)
        {
            lblVel2.Text = numVelPos.Text;
        }

       

        private void btnConnectedRouter_Click(object sender, EventArgs e)
        {
            if (btnConnectedRouter.Text == "Open")
            {
                if (Router.IsOpen)
                {
                   Router.Close();
                }
                if (!Router.Open(cbxComLed.Text, int.Parse(cbxBaudRateLed.Text), int.Parse(cbxDataBitsLed.Text), (Parity)Enum.Parse(typeof(Parity), cbxParityLed.Text)))
                {
                    MessageBox.Show("Kết Nối Không Thành Công");

                }
                else
                {
                    btnConnectedRouter.Text = "Close";
                    MessageBox.Show("Kết Nối  Thành Công");
                }

            }
            else
            {
                btnConnectedRouter.Text = "Open";
                Router.Close();
            }
        }

        private void btnConnectedLed_Click(object sender, EventArgs e)
        {
            if (btnConnectedLed.Text == "Open")
            {
                if(Led.IsOpen)
                {
                    Led.Close();
                }
                if (!Led.Open(cbxComLed.Text, int.Parse(cbxBaudRateLed.Text), int.Parse(cbxDataBitsLed.Text), (Parity)Enum.Parse(typeof(Parity), cbxParityLed.Text)))
                {
                    MessageBox.Show("Kết Nối Không Thành Công");
                   
                }
                else
                {
                    btnConnectedLed.Text = "Close";
                    MessageBox.Show("Kết Nối  Thành Công");
                }

            }
            else
            {
                btnConnectedLed.Text = "Open";
                Led.Close();
            }
            
          
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to Run Manual?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Data_motion Data_motion = Config_Motion.prameter.Data_motion_config[selected_row_index];
                
               Setting_Control.Instance.StartMoveAbs(Data_motion.Vel, Data_motion.Acc, Data_motion.Decc, Data_motion.Position, (Int16)comboBox1.SelectedIndex);
                
            }
           
        }

        private void Teach_pos_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCurrentPos.Text == "" || lblCurrentPos.Text == null)
                {
                    MessageBox.Show("Current Position is Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Managa_pos.CurrentRow.Cells["Position"].Value = lblCurrentPos.Text;
            }
            catch
            {
                MessageBox.Show("Please Add Position");
            }
          
        }

        private void guna2VTrackBar1_ValueChanged(object sender, EventArgs e)
        {
            lock (_Lock_Led)
            {

                //if (flag_Led&&Led.IsOpen)
                //{
                //    flag_Led=false;
                //    int lastValue = guna2VTrackBar1.Value;
                //    if (Current_Value_Led < lastValue)
                //    {
                //        Task.Run(() =>
                //        {

                //            for (int i = Current_Value_Led; i <= lastValue; i++)
                //            {
                                Led.Send($"SA{guna2VTrackBar1.Value.ToString("D4")}TC#");
                    //            Task.Delay(50);
                    //        }

                    //        flag_Led = true;
                    //    });
                    //}
                    //else
                    //{
                    //    Task.Run(() =>
                    //    {

                    //        for (int i = Current_Value_Led; i >= lastValue; i--)
                    //        {

                    //            Led.Send($"SA{i.ToString("D4")}TC#");
                    //            Task.Delay(50);
                    //        }

                    //    });
                    //    flag_Led = true;
                    //}
                //}

            }
            lblRangeLed.Text = guna2VTrackBar1.Value.ToString();

        }

        private void button28_Click(object sender, EventArgs e)
        {
            Int16 nEcatIoBitNo = Convert.ToInt16(textBox19.Text);
            if (button28.Text == "ON")
            {
               
                ret = ImcApi.IMC_SetEcatDoBit(nCardHandle, nEcatIoBitNo, 1);
                if (ret != 0)
                {
                    if (lbLog.Items.Count > 500)
                    {
                        lbLog.Items.RemoveAt(0);
                    }
                    lbLog.Items.Add($"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss:fff")} : " + "Quá trình thiết lập ECAT DO thất bại với mã lỗi 0x." + ret.ToString("x8"));
                    LogProgram.MesWriteLog($"Quá trình thiết lập ECAT DO thất bại với mã lỗi 0x.{ret.ToString("x8")}");
                    return;
                }
                button28.Text = "OFF";
        
            }
            else
            {
               
                ret = ImcApi.IMC_SetEcatDoBit(nCardHandle, nEcatIoBitNo, 0);
                if (ret != 0)
                {
                    if (lbLog.Items.Count > 500)
                    {
                        lbLog.Items.RemoveAt(0);
                    }
                    lbLog.Items.Add($"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss:fff")} : " + "Quá trình thiết lập ECAT DO thất bại với mã lỗi 0x." + ret.ToString("x8"));
                    LogProgram.MesWriteLog($"Quá trình thiết lập ECAT DO thất bại với mã lỗi 0x.{ret.ToString("x8")}");
                    return;
                }
            
                button28.Text = "ON";
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path_data_model_folder = path_manager + "\\" + Config_Model.model.Current_model;
            string path_data_model = path_data_model_folder + "\\" + Config_Model.model.Current_model + ".json";
            _CYLINDER = Config_Motion.prameter._cylinder.FirstOrDefault(x => x.No == 1);
            _CYLINDER.BIT_CYLINDER = int.Parse(textBox19.Text);
            Config_Motion.SaveToJson(path_data_model);
        }

        private void label60_TextChanged(object sender, EventArgs e)
        {
            lblCurrentPos.Text = (double.Parse(label60.Text)/100).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path_data_model_folder = path_manager + "\\" + Config_Model.model.Current_model;
            string path_data_model = path_data_model_folder + "\\" + Config_Model.model.Current_model + ".json";
            _HOME = Config_Motion.prameter._Home.FirstOrDefault();
            _HOME.Home_Method = int.Parse(txtHome.Text);
            _HOME.OffSet = int.Parse(txtOffSet.Text);
            _HOME.LowVel = int.Parse(txtLowVel.Text)*100;
            _HOME.HighVel = int.Parse(txtHighVel.Text) * 100;
            _HOME.ACC_DECC = int.Parse(txtAccDec.Text);
            Config_Motion.SaveToJson(path_data_model);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
    
            string path_data_model_folder = path_manager + "\\" + Config_Model.model.Current_model;
            string path_data_model = path_data_model_folder + "\\" + Config_Model.model.Current_model + ".json";
            serialRouter = Config_Motion.prameter.Serial_configs.FirstOrDefault(x => x.DeviceName == "Router");
            serialRouter.DeviceName = "Router";
            serialRouter.PortName = cbxComRouter.Text;
            serialRouter.BaudRate = int.Parse(cbxBaudRateRouter.Text);
            serialRouter.DataBits = int.Parse(cbxDataBitsRouter.Text);
            serialRouter.Parity = (Parity)Enum.Parse(typeof(Parity), cbxParityRouter.Text);
            serialRouter.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbxStopBitRouter.Text);
            serialRouter.AutoConnected = cbAutoConnectedRouter.Checked;
            Config_Motion.SaveToJson(path_data_model);
        }

        private void btnSaveLed_Click(object sender, EventArgs e)
        {
            string path_data_model_folder = path_manager + "\\" + Config_Model.model.Current_model;
            string path_data_model = path_data_model_folder + "\\" + Config_Model.model.Current_model + ".json";
            serialLed = Config_Motion.prameter.Serial_configs.FirstOrDefault(x => x.DeviceName == "Led");
            serialLed.DeviceName = "Led";
            serialLed.PortName = cbxComLed.Text;
            serialLed.BaudRate = int.Parse(cbxBaudRateLed.Text);
            serialLed.DataBits = int.Parse(cbxDataBitsLed.Text);
            serialLed.Parity = (Parity)Enum.Parse(typeof(Parity), cbxParityLed.Text);
            serialLed.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbxStopBitLed.Text);
            serialLed.AutoConnected =cbAutoConnectedLed.Checked;
            Config_Motion.SaveToJson(path_data_model);
        }

        #region VARIABLE
        // JOG
        private decimal ScaleValue
        {
            get
            {
                return numVelJog.Value * 100; 
            }
        }

        private void btnSaveLocalFile_Click(object sender, EventArgs e)
        {
            Manager_path.Creat_path(path_manager);
            Config_FileLog._FileLocal.LogDirectory = txtLocalFile.Text;
            Config_FileLog.SaveToJson(path_LocalFile);

        }

        private void Reset_mode_Click(object sender, EventArgs e)
        {
            if (Setting_Control.Instance.InitCard())
            {
                Setting_Control.Instance.StartHome(short.Parse(txtHome.Text), int.Parse(txtOffSet.Text), double.Parse(txtHighVel.Text) * 100, double.Parse(txtLowVel.Text) * 100, double.Parse(txtAccDec.Text));
                btnServo.Enabled = true;
            }
            else
            {
                btnServo.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(btnServo.Text== "Servo OFF")
            {
                btnServo.Text = "Servo ON";
                Setting_Control.Instance.ServoOff();
            }
            else
            {
                btnServo.Text = "Servo OFF";
                Setting_Control.Instance.ServoOn();
            }
        }

        private void Jog_positive_Click(object sender, EventArgs e)
        {

        }

        // Current Value
        private decimal ScaleVelPos
        {
            get
            {
                return numVelPos.Value*100;
            }
        }

        // Pos
        private decimal ScalePos
        {
            get
            {
                return numTargetPos.Value * 100;
            }
        }
        #endregion
    }

}
