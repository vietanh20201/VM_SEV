using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC_Vision.Manager_path
{
    public static class Manager_path
    {
        public static string path_manager = "C:\\Data_base_motion_DB\\Model";

        public static string path_communication = "C:\\Data_base_motion_DB\\Communication";
        public static void Creat_path(string NameFolder) 
        {
            DirectoryInfo Dir = new DirectoryInfo(NameFolder);
            if (!Dir.Exists)
            {
                Dir.Create();
            }
        }
    }
}
