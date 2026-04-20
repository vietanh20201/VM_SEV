using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision_motion.MOTION.Check_program
{
    public class SingleInstanceApplication : WindowsFormsApplicationBase

    {

        private SingleInstanceApplication()

        {

            base.IsSingleInstance = true;

        }

        public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
        {

            SingleInstanceApplication app = new SingleInstanceApplication();

            app.MainForm = f;

            app.StartupNextInstance += startupHandler;

            app.Run(Environment.GetCommandLineArgs());
        }

    }

}
