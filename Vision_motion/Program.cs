using Bonding_Vision;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vision_motion.MOTION.Check_program;

namespace Vision_motion
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static Form_Main Form_main;
        [STAThread]
        static void Main(string[] args)
        {

            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //SplashScreenManager.ShowForm(typeof(Form_initial_wait));
                Form_main = new Form_Main();
                SingleInstanceApplication.Run(Form_main, NewInstanceHandler1);

            }
            catch (Exception ex)
            {
                string str = GetExceptionMsg(ex, string.Empty);
                MessageBox.Show(str, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        public static void NewInstanceHandler1(object sender,
            StartupNextInstanceEventArgs e)
        {
            //SplashScreenManager.ShowForm(typeof(Form_wait));
            Form_main.Activate();
            //SplashScreenManager.CloseForm();
        }

        private static void Application_ThreadException(object sender,
            ThreadExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.Exception, e.ToString());
            WriteLog(str);
            MessageBox.Show(str, "system error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private static void CurrentDomain_UnhandledException(object sender,
            System.UnhandledExceptionEventArgs e)
        {
            string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
            WriteLog(str);
            MessageBox.Show(str, "system Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        private static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************unusual text****************************");
            sb.AppendLine("【Appearance time】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【Exception type】：" + ex.GetType().Name);
                sb.AppendLine("【Exception information】：" + ex.Message);
                sb.AppendLine("【stack call】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【Unhandled exception】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
        private static void WriteLog(string str)
        {
            DirectoryInfo dinfo = new DirectoryInfo("C:\\AMAGROUP\\error log\\");
            if (!dinfo.Exists)
            {
                dinfo.Create();
            }
            FileStream fs = new FileStream("C:\\AMAGROUP\\error log\\" + DateTime.Now.ToString("MMddHHmmss") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(str);
            sw.Close();
            fs.Close();
        }
    }
}
