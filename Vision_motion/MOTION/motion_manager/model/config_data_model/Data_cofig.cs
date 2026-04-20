using Newtonsoft.Json;
using PC_Control_SEV.Motion_Manager.model.config_data_model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QC_Vision.Config_data_model
{
    public static class Config_Motion
    {
        //
        public static prameter_Motion prameter = new prameter_Motion();
        public static void SaveToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(prameter,Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        public static prameter_Motion LoadFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<prameter_Motion>(json);
        }
    }
    public class prameter_Motion
    {
        public string[] NameCam { get; set; }
        public List<List<string>> Mapping = new List<List<string>>();
        public List<List<List<string>>> Function_tool = new List<List<List<string>>>();
        public List<Data_motion> Data_motion_config = new List<Data_motion>();
        BindingList<Data_motion> motions = new BindingList<Data_motion>();
        public List<Data_Router_Led> Serial_configs = new List<Data_Router_Led>();
        public List<CYLINDER> _cylinder = new List<CYLINDER>();
        public List<HOME> _Home = new List<HOME>();
        public List<LocalFileLog> _LocalFileLog = new List<LocalFileLog>();

    }
}
