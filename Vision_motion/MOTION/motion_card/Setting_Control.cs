using HarfBuzzSharp;
using Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi;
using QC_Vision.Config_data_model;
using System;
using System.Linq;

namespace PC_Control_SEV.Motion_Card
{
    public enum Status
    {
        Idle,
        Homing,
        Moving,
        Error,
        Success
    }

    public class Setting_Control
    {
        public static readonly object _lock = new object();
        private static Setting_Control _instance;

        public static Setting_Control Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Setting_Control();
                    return _instance;
                }
            }
        }

        public Action<string> Logs;
        public event Action<string, object> AxisStatusChanged;

        private UInt32 ret = 0;
        private UInt64 cardHandle = 0;
        private short axisNo = 0;

        public Status CurrentState { get; private set; } = Status.Idle;

        private int[] nTimerAxSts = new int[1];
        private double[] nTimerAxPrfPos = new double[1];
        private double[] nTimerAxPrfVel = new double[1];
        private double[] nTimerAxEncPos = new double[1];
        private double[] nTimerAxEncVel = new double[1];
        private int nTimerAxInput;

        #region INIT

        public bool InitCard()
        {
            int cardNum = 0;

            ret = ImcApi.IMC_GetCardsNum(ref cardNum);
            if (ret != 0 || cardNum <= 0)
            {
                Main_control._Alarm?.Invoke("Không tìm thấy card");
                return false;
            }

            ret = ImcApi.IMC_OpenCardHandle(0, ref cardHandle);
            if (ret != 0)
            {
                Main_control._Alarm?.Invoke($"Open card fail 0x{ret:x8}");
                return false;
            }

            if (ImcApi.IMC_DownLoadDeviceConfig(cardHandle, ".\\Config_Motion\\device_config.xml") != 0)
            {
                Main_control._Alarm?.Invoke("Load device config fail");
                return false;
            }

            if (ImcApi.IMC_DownLoadSystemConfig(cardHandle, "..\\Config_Motion\\system_config.xml") != 0)
            {
                Main_control._Alarm?.Invoke("Load system config fail");
                return false;
            }

            ServoOn();
            return true;
        }

        public void Close_Card()
        {
            ImcApi.IMC_CloseCard(cardHandle);
        }

        #endregion

        #region SERVO

        public void ServoOn()
        {
            if (ImcApi.IMC_AxServoOn(cardHandle, axisNo, 1) != 0)
            {
                Log("Servo ON fail");
                CurrentState = Status.Error;
                return;
            }
            Log("Servo ON");
        }

        public void ServoOff()
        {
            if (ImcApi.IMC_AxServoOn(cardHandle, axisNo, 0) != 0)
            {
                Log("Servo OFF fail");
                return;
            }
            Log("Servo OFF");
        }

        #endregion

        #region HOME

        public bool StartHome(short homeValue, int offset, double highVel, double lowVel, double acc)
        {
            if (CurrentState != Status.Idle)
            {
                Log("Axis đang bận!");
                return false;
            }

            ImcApi.THomingPara home = new ImcApi.THomingPara
            {
                homeMethod = homeValue,
                offset = offset,
                highVel = (uint)highVel,
                lowVel = (uint)lowVel,
                acc = (uint)acc
            };

            ret = ImcApi.IMC_StartHoming(cardHandle, axisNo, ref home);

            if (ret != 0)
            {
                Main_control._Alarm?.Invoke($"Homing fail 0x{ret:x8}");
                CurrentState = Status.Error;
                return false;
            }

            CurrentState = Status.Homing;
            Log("Start Homing...");
            return true;
        }

        #endregion

        #region JOG 

        public void JogMinusDown(double vel)
        {
            double tgtVel = -Math.Abs(vel);
            ImcApi.IMC_StartJogMove(cardHandle, axisNo, tgtVel);
        }

        public void JogMinusUp()
        {
            ImcApi.IMC_AxMoveStop(cardHandle, axisNo, 1);
        }

        public void JogPlusDown(double vel)
        {
            double tgtVel = Math.Abs(vel);
            ImcApi.IMC_StartJogMove(cardHandle, axisNo, tgtVel);
        }

        public void JogPlusUp()
        {
            ImcApi.IMC_AxMoveStop(cardHandle, axisNo, 1);
        }

        #endregion

        public void EmergencyStop()
        {
            ImcApi.IMC_SetEmgTrigLevelInv(cardHandle, 0);
            CurrentState = Status.Error;
            Log("EMG Triggered");
        }

        #region MOVE

        public bool StartMoveAbs(double vel, double acc, double dec, double pos, short postype)
        {
            if (CurrentState != Status.Idle)
            {
                Log("Axis đang bận!");
                return false;
            }

            ret = ImcApi.IMC_SetSingleAxMvPara(cardHandle, 0, vel, acc, dec);
            if (ret != 0)
            {
                Main_control._Alarm?.Invoke($"Set param fail 0x{ret:x8}");
                CurrentState = Status.Error;
                return false;
            }

            ret = ImcApi.IMC_StartPtpMove(cardHandle, axisNo, pos, postype);
            if (ret != 0)
            {
                Main_control._Alarm?.Invoke($"Move fail 0x{ret:x8}");
                CurrentState = Status.Error;
                return false;
            }

            CurrentState = Status.Moving;
            Log("Start Move...");
            return true;
        }

        #endregion

        #region MONITOR

        public void MonitorAxis()
        {
            if (ImcApi.IMC_GetAxSts(cardHandle, axisNo, nTimerAxSts, 1) != 0)
            {
                CurrentState = Status.Error;
                Log("Lỗi đọc trạng thái trục!");
                return;
            }

            ImcApi.IMC_GetAxPrfPos(cardHandle, axisNo, nTimerAxPrfPos, 1);
            ImcApi.IMC_GetAxPrfVel(cardHandle, axisNo, nTimerAxPrfVel, 1);
            ImcApi.IMC_GetAxEncPos(cardHandle, axisNo, nTimerAxEncPos, 1);
            ImcApi.IMC_GetAxEncVel(cardHandle, axisNo, nTimerAxEncVel, 1);
            ImcApi.IMC_GetAxEcatDigitalInput(cardHandle, axisNo, ref nTimerAxInput);

            int sts = nTimerAxSts[0];

            bool isBusy = (sts & 0x04) != 0;
            bool isArrive = (sts & 0x08) != 0;

            if (CurrentState == Status.Moving && !isBusy && isArrive)
            {
                CurrentState = Status.Idle;
                Log("Move Done");
            }

            if (CurrentState == Status.Homing)
            {
                short status = 0;

                if (ImcApi.IMC_GetHomingStatus(cardHandle, axisNo, ref status) == 0)
                {
                    if (status == 3)
                    {
                        CurrentState = Status.Idle;
                        Log("Home Done");
                    }
                    else if (status < 0)
                    {
                        CurrentState = Status.Error;
                        Log("Home Fail");
                    }
                }
            }

            Notify("label60", nTimerAxPrfPos[0]);
            Notify("lblVel", nTimerAxPrfVel[0]);
        }

        #endregion

        #region CYLINDER 

        public Status CLAMPING_CYLINDER()
        {
            var lst = Config_Motion.prameter._cylinder.FirstOrDefault(x => x.No == 1);
            Int16 nEcatIoBitNo = Convert.ToInt16(lst.BIT_CYLINDER);

            ret = ImcApi.IMC_SetEcatDoBit(cardHandle, nEcatIoBitNo, 1);
            if (ret != 0) return Status.Error;

            return Status.Success;
        }

        public Status RELEASE_CYLINDER()
        {
            var lst = Config_Motion.prameter._cylinder.FirstOrDefault(x => x.No == 1);
            Int16 nEcatIoBitNo = Convert.ToInt16(lst.BIT_CYLINDER);

            ret = ImcApi.IMC_SetEcatDoBit(cardHandle, nEcatIoBitNo, 0);
            if (ret != 0) return Status.Error;

            return Status.Success;
        }

        #endregion

        #region OTHER

        public void Stop()
        {
            if (ImcApi.IMC_AxMoveStop(cardHandle, axisNo, 1) != 0)
            {
                Log("Stop fail");
                CurrentState = Status.Error;
                return;
            }

            CurrentState = Status.Idle;
            Log("Stop OK");
        }

        private void Notify(string name, object value)
        {
            AxisStatusChanged?.Invoke(name, value);
        }

        private void Log(string msg)
        {
            Logs?.Invoke(msg);
        }

        #endregion
    }
}