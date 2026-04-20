using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace QC_Vision.Model
{
    public class Model
    {
        public string Current_model { get; set;}
        public List<string> List_model = new List<string>();
        public string Model_select = "";

    }
    public class LocalFileLog
    {
        public string LogDirectory { get; set; } = "Application.StartupPath";

    }
    public class Login
    {
        public string Pass_Word { get; set; } = "SEV2026";

    }
    public static class Config_FileLog
    {
        public static LocalFileLog _FileLocal = new LocalFileLog();

        public static void SaveToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(_FileLocal, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static LocalFileLog LoadFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<LocalFileLog>(json);
        }


    }
    public static class Config_Login
    {
        public static Login _Login = new Login();

        public static void SaveToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(_Login, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static Login LoadFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Login>(json);
        }


    }

    public static class Config_Model
    {
        public static Model model = new Model();

        public static void SaveToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static Model LoadFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Model>(json);
        }

        public static void Delete_model()
        {
            string current_model= model.Current_model;
            string model_select = model.Model_select;
            string path_fodel_model=Manager_path.Manager_path.path_manager+
                "\\"+ model_select;
            if (Directory.Exists(path_fodel_model))
            {
                Directory.Delete(path_fodel_model, true);
            }
            int index = model.List_model.IndexOf(model_select);
            if (index != -1)
            {
                model.List_model.Remove(model_select);
                string path_manager = Manager_path.Manager_path.path_manager;
                string path_model = path_manager + "\\model.json";
                Config_Model.SaveToJson(path_model);
            }

        }
    }
}
