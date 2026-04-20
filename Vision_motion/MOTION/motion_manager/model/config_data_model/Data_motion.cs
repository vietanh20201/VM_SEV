using QC_Vision.Config_data_model;
using QC_Vision.Paramester_tool_UI.Converter_Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Control_SEV.Motion_Manager.model.config_data_model
{
    public class Data_motion 
    {
        [Category("Parameter")]
        [DisplayName("Position")]
        [Editor(typeof(NumericEditor), typeof(UITypeEditor))]
     
        public double Position { get; set; } = 100.0;

       


        [Category("Name Position")]
        [Display(Name = "Name_Position")]
        [TypeConverter(typeof(StringNumberEditor))]
   
        public string Name_Position { get; set; } = "Test";



        [Category("Process")]
      
        [TypeConverter(typeof(TypeProcessConverter))]
    
        public string OK { get; set; }
        [Category("Process")]

        [TypeConverter(typeof(TypeProcessConverter))]
       
        public string NG { get; set; }


        [Category("Process")]
        [TypeConverter(typeof(TypeProcessConverter))]
   
        public string Error { get; set; }

        [Category("Process")]
        [DisplayName("Delay(s)")]
        [Editor(typeof(NumericEditor), typeof(UITypeEditor))]

        public double Delay { get; set; } = 0.5;

        [Category("Process")]
        [TypeConverter(typeof(EventProcess))]
       
        public string ProcessEvent { get; set; } = "Wait";
        [Category("Process")]
        [TypeConverter(typeof(StringNumberEditor))]
       
        public string FeedBack { get; set; } = "None";
        [Category("Parameter")]
        [DisplayName("TimeOut")]
        [Editor(typeof(NumericEditor), typeof(UITypeEditor))]
       
        public double TimeOut { get; set; } = 1000.0;

        [Category("Vận Tốc /Tăng Giảm Tốc")]
        [Editor(typeof(NumericEditor), typeof(UITypeEditor))]
      
        public double Acc { get; set; } = 2500;
        [Category("Tăng Giảm Tốc")]
       
        public double Decc { get; set; } = 2500;
        [Category("Parameter")]
        [DisplayName("Vel")]
        [Editor(typeof(NumericEditor), typeof(UITypeEditor))]
       
        public double Vel { get; set; } = 2500;




    }
}
