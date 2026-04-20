using QC_Vision.Config_data_model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using static System.ComponentModel.TypeConverter;
namespace QC_Vision.
    Paramester_tool_UI.Converter_Property
{
    public class MasterConverter : StringConverter//Master
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_property_grid.List_master != null)
            {
                return new StandardValuesCollection(list_paramester_property_grid.List_master);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }//Master
    public class NumericEditor : UITypeEditor//Numeric
    {
        public override UITypeEditorEditStyle
            GetEditStyle(ITypeDescriptorContext context)
            => UITypeEditorEditStyle.DropDown;
        public override object
            EditValue(ITypeDescriptorContext
            context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = 
                provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (editorService != null)
            {
                NumericUpDown nud = new NumericUpDown
                {
                    Minimum = -100000,
                    Maximum = 100000,
                    Value = Convert.ToDecimal(value),
                    DecimalPlaces = 3
                };
                editorService.DropDownControl(nud);
                return (int)nud.Value;
            }
            return value;
        }
    }//Numeric
    public class StringNumberEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            => UITypeEditorEditStyle.DropDown;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService =
                provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (editorService != null)
            {
                TextBox textBox = new TextBox
                {
                    Text = value?.ToString(),
                    BorderStyle = BorderStyle.None,
                    Width = 100
                };

                // Optional: Xử lý sự kiện khi người dùng nhấn Enter
                textBox.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        editorService.CloseDropDown();
                    }
                };

                editorService.DropDownControl(textBox);
                return textBox.Text;
            }

            return value;
        }
    }//String
    public class BoolEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            => UITypeEditorEditStyle.DropDown;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (editorService == null) return value;

            ComboBox comboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Thêm 2 lựa chọn true/false
            comboBox.Items.Add("True");
            comboBox.Items.Add("False");

            // Set lựa chọn hiện tại
            comboBox.SelectedItem = value?.ToString() ?? "False";

            // Khi chọn xong thì đóng dropdown
            comboBox.SelectedValueChanged += (s, e) =>
            {
                editorService.CloseDropDown();
            };

            editorService.DropDownControl(comboBox);

            return bool.TryParse(comboBox.SelectedItem.ToString(), out bool result) ? result : value;
        }
    }//Boolean
    public class Type_find_circle_Converter : //Type find circle 
        StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_property_grid.List_type_circle != null)
            {
                return new StandardValuesCollection(list_paramester_property_grid.List_type_circle);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }//Type find circle 

    public class ProcessItem
    {
        [DisplayName("Next")]
        [TypeConverter(typeof(TypeProcessConverter))]
        public string Next { get; set; } = "Next";

        [DisplayName("Retry")]
        [TypeConverter(typeof(TypeProcessConverter))]
        public string Retry { get; set; } = "Next";

        [DisplayName("Stop")]
        [TypeConverter(typeof(TypeProcessConverter))]
        public string Stop { get; set; } = "End";

        // hiển thị gọn khi chưa expand
        public override string ToString()
        {
            return "Action";
        }
    }
    public class TypeProcessConverter : 
        StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
    public override bool
        GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
    public override
        StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        if (context?.Instance != null && list_paramester_property_grid.TypeProcess != null)
        {
              
                foreach (var item in Config_Motion.prameter.Data_motion_config)
                {
                  if(item.Name_Position!=string.Empty)
                    {
                        if (!list_paramester_property_grid.TypeProcess.Contains("Tostep : " + item.Name_Position))
                        {
                           list_paramester_property_grid.TypeProcess.Add("Tostep : " + item.Name_Position);
                        }
                    }
                }
                return new StandardValuesCollection(list_paramester_property_grid.TypeProcess);
        }
        return new StandardValuesCollection(new List<string>());
    }
}
    public class EventProcess :
           StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_property_grid.TypeProcess != null)
            {

                return new StandardValuesCollection(list_paramester_property_grid.EventProcess);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }
    public class Light_to_dark_Converter ://Type find circle 
        StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_property_grid.Light_to_dark != null)
            {
                return new StandardValuesCollection(list_paramester_property_grid.Light_to_dark);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }//Type find circle 
    public class Name_check_Converter ://Name check
        StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_property_grid.Name_check != null)
            {
                return new StandardValuesCollection(list_paramester_property_grid.Name_check);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }//Name check
    public class FolderBrowserEditor : UITypeEditor// Sellect folder
    {
        public override UITypeEditorEditStyle 
            GetEditStyle(ITypeDescriptorContext context)
            => UITypeEditorEditStyle.Modal;

        public override object EditValue
            (ITypeDescriptorContext context, 
            IServiceProvider provider, object value)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Sellect folder";
                if (value is string path && !string.IsNullOrEmpty(path))
                {
                    dialog.SelectedPath = path;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }

            return value;
        }
    }// Sellect folder
    public class OMCFileSelectorEditor : UITypeEditor//Sellect file OMC (OCR find text )
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            => UITypeEditorEditStyle.Modal;

        public override object EditValue(ITypeDescriptorContext context,
                                         IServiceProvider provider,
                                         object value)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select OMC file";
                dialog.Filter = "OCR Model (*.omc)|*.omc|All files (*.*)|*.*";
                dialog.CheckFileExists = true;

                if (value is string path && !string.IsNullOrEmpty(path))
                {
                    dialog.FileName = path;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }

            return value;
        }
    }
    public class Class_name_hal_Converter : StringConverter//Classify hal
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_property_grid.Class_name_hal != null)
            {
                return new StandardValuesCollection(list_paramester_property_grid.Class_name_hal);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }//Classify hal
    //public class Type_rename_Converter ://Name check
    //    StringConverter
    //{
    //    public override bool
    //        GetStandardValuesSupported(ITypeDescriptorContext context) => true;
    //    public override bool
    //        GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
    //    public override
    //        StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        if (context?.Instance != null && list_paramester_property_grid.Type_rename != null)
    //        {
    //            return new StandardValuesCollection(list_paramester_property_grid.Type_rename);
    //        }
    //        return new StandardValuesCollection(new List<string>());
    //    }
    //}//Name check
    public class Type_pixel: StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_cam_property_select.Pixel_type != null)
            {
                return new StandardValuesCollection(list_paramester_cam_property_select.Pixel_type);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }
    public class Type_exposure_time : StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_cam_property_select.Type_exposure_time != null)
            {
                return new StandardValuesCollection(list_paramester_cam_property_select.Type_exposure_time);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }
    public class Type_gain : StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_cam_property_select.Type_gain != null)
            {
                return new StandardValuesCollection(list_paramester_cam_property_select.Type_gain);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }

    public class Serial_camera : StringConverter
    {
        public override bool
            GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        public override bool
            GetStandardValuesExclusive(ITypeDescriptorContext context) => true;
        public override
            StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context?.Instance != null && list_paramester_cam_property_select.Serial_camera != null)
            {
                return new StandardValuesCollection(list_paramester_cam_property_select.Serial_camera);
            }
            return new StandardValuesCollection(new List<string>());
        }
    }
    public static class list_paramester_property_grid
    {
        [Browsable(false)] // Ẩn danh sách địa chỉ trong PropertyGrid
        public static List<string> List_master { get; set; }=
        new List<string>
        {
                "None"
        };
        public static List<string> List_type_circle { get; set; } =
        new List<string>
        {
                "all","first","last"
        };
        public static List<string> Light_to_dark { get; set; } =
        new List<string>
        {
                "positive","negative","uniform"
        };
        public static List<string> Name_check { get; set; } =
        new List<string>
        {
                "None"
        };
        public static List<string> Type_rename { get; set; } =
        new List<string>
        {
                "GUID","Sequential"
        };
        public static List<string> TypeProcess { get; set; } =
        new List<string>
        {
                "next","end"
        };
        public static List<string> EventProcess { get; set; } =
      new List<string>
      {
                "Wait","Move","Trigger","CLAMPING_CYLINDER","RELEASE_CYLINDER"
      };
        public static List<string> Class_name_hal { get; set; } =
        new List<string>();

    }// search master import property grid
    public static class list_paramester_cam_property_select
    {
        [Browsable(false)] // Ẩn danh sách địa chỉ trong PropertyGrid
        public static List<string> Pixel_type { get; set; } =
        new List<string>
        {
                "PixelType_Gvsp_Mono8",
                "PixelType_Gvsp_YUV422_YUYV_Packed",
                "PixelType_Gvsp_YUV422_Packed",
                "PixelType_Gvsp_BayerBG8"
        };
        public static List<string> Type_exposure_time { get; set; } =
        new List<string>
        {
                "ExposureTime",
                "ExposureTimeAbs"
        };
        public static List<string> Type_gain { get; set; } =
        new List<string>
        {
                "Gain",
                "GainRaw"
        };
        public static List<string> Serial_camera { get; set; } =
        new List<string>
        {
                ""
        };
    }// Camera config

}
