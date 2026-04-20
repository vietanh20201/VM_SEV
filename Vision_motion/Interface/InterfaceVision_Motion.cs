using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_motion.Interface
{
    public class InterfaceVision_Motion
    {
        private static bool _trigger;
        public static bool Trigger // Team Motion Start Trigger = true ,sau khi trigger trong 1 khoảng thời gian delay sẽ lật về false
        {
            get => _trigger;
            set
            {
                if (_trigger == value) return;
                _trigger = value;
            }
        }

        private static bool _triggerDone;
        public static bool Trigger_Done // Team Vision cần báo trigger thành công khi trigger xong và có kết quả 
        {
            get => _triggerDone;
            set
            {
                if (_triggerDone == value) return;
                _triggerDone = value;
            }
        }
        private static bool _endProcess;
        public static bool EndProcess // Sau khi lấy kết quả xong thì Team Motion sẽ true EndProcess trong 1 khoảng thời gian . Team Vision cần đổi _Status sang None 
        {
            get => _triggerDone;
            set
            {
                if (_triggerDone == value) return;
                _triggerDone = value;
            }
        }

        private static Status _status;
        public static Status _Status   // Kết quả khi trigger xong 
        {
            get => _status;
            set
            {
                if (_status == value) return;
                _status = value;
            }
        }
    }

    public enum Status
    {
        OK,
        NG1,  // Kênh
        NG2,  // Thiếu sản phẩm
        NG3,   // NG đường cắt
        None,
    }
}

