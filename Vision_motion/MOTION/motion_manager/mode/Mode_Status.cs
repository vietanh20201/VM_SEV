using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Control_SEV.Motion_Manager.mode
{
    public static class Mode_Status
    {
        public static bool Auto_Manual = false;// false: Manual, true: Auto
        public static bool Origin_runing = false;// false: null origin, true: running origin complete
    }
}
