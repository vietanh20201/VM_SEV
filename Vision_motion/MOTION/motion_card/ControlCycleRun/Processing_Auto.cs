using PC_Control_SEV.Motion_Manager.model.config_data_model;
using QC_Vision.Config_data_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace PC_Control_SEV.Motion_Card.ControlCycleRun
{
    public class Processing_Auto
    {
        private bool _isRunning = false;

        public static Func<Data_motion, string> OnWaitProcess;
        public static Func<Data_motion, string> OnTriggerProcess;

        public void Start_Process()
        {
            if (_isRunning) return;

            _isRunning = true;

            Thread thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void Run()
        {
            var steps = Config_Motion.prameter.Data_motion_config;

            if (steps == null || steps.Count == 0)
                return;

            int currentIndex = 0;

            while (_isRunning)
            {
                if (currentIndex < 0 || currentIndex >= steps.Count)
                    currentIndex = 0;

                var step = steps[currentIndex];

                Console.WriteLine($"Running step: {step.Name_Position}");

                DateTime stepStart = DateTime.Now;
                string result = null;
                if (Enum.TryParse<Process>(step.ProcessEvent?.Trim().ToLower(), out Process _process))
                {
                    switch (_process)
                    {
                        case Process.MOVE:
                            result = ProcessMove(step, stepStart);
                            break;

                        case Process.Wait:
                            result = ProcessWait(step, stepStart);
                            break;

                        case Process.TRIGGER:
                            result = ProcessTrigger(step, stepStart);
                            break;
                        case Process.CLAMPING_CYLINDER:
                            result = ProcessTrigger(step, stepStart);
                            break;
                        case Process.RELEASE_CYLINDER:
                            result = ProcessTrigger(step, stepStart);
                            break;

                        default:
                            Console.WriteLine("ProcessEvent không hợp lệ → Error");
                            result = "Error";
                            break;
                    }
                }

                Console.WriteLine($"Result: {result}");

                switch (result)
                {
                    case "OK":
                        currentIndex = HandleResult(step.OK, steps, currentIndex);
                        break;

                    case "NG":
                        currentIndex = HandleResult(step.NG, steps, currentIndex);
                        break;

                    case "Error":
                        currentIndex = HandleResult(step.Error, steps, currentIndex);
                        break;
                }
            }
        }

       
        private string ProcessMove(Data_motion step, DateTime stepStart)
        {
            var status = Setting_Control.Instance.StartMoveAbs(
                vel: step.Vel,
                acc: step.Acc,
                dec: step.Decc,
                pos: step.Position,
                postype: 0
            );

            if (status == false)
                return "Error";

            Thread.Sleep((int)(step.Delay * 1000));

            if (IsTimeout(stepStart, step.TimeOut))
                return "Error";

            return "OK";
        }

       
        private string ProcessWait(Data_motion step, DateTime stepStart)
        {
            while (true)
            {
             
                if (IsTimeout(stepStart, step.TimeOut))
                    return "Error";

             
                string feedback = OnWaitProcess?.Invoke(step);

                if (feedback != null)
                {
                  

                    if (string.IsNullOrEmpty(step.FeedBack) || step.FeedBack == "None")
                        return "OK";

                   
                    if (feedback.Trim() == step.FeedBack.Trim())
                        return "OK";
                    else
                        return "NG";
                }

                Thread.Sleep(10);
            }
        }

     
        private string ProcessTrigger(Data_motion step, DateTime stepStart)
        {
            if (OnTriggerProcess != null)
            {
                var result = OnTriggerProcess(step);

                if (result != null)
                    return result;
            }

            while (true)
            {
                if (IsTimeout(stepStart, step.TimeOut))
                    return "Error";

                if (OnTriggerProcess != null)
                {
                    var result = OnTriggerProcess(step);
                    if (result != null)
                        return result;
                }

                Thread.Sleep(10);
            }
        }
        private string CLAMPING_CYLINDER(Data_motion step, DateTime stepStart)
        {

            var status = Setting_Control.Instance.RELEASE_CYLINDER();

            if (status == Status.Error)
                return "Error";

            Thread.Sleep((int)(step.Delay * 1000));

            if (IsTimeout(stepStart, step.TimeOut))
                return "Error";

            return "OK";
        }
        private string RELEASE_CYLINDER(Data_motion step, DateTime stepStart)
        {
            var status = Setting_Control.Instance.RELEASE_CYLINDER();

            if (status == Status.Error)
                return "Error";

            Thread.Sleep((int)(step.Delay * 1000));

            if (IsTimeout(stepStart, step.TimeOut))
                return "Error";

            return "OK";
        }


        private bool IsTimeout(DateTime start, double timeoutMs)
        {
            return (DateTime.Now - start).TotalMilliseconds > timeoutMs;
        }

        private int HandleResult(string action, List<Data_motion> steps, int currentIndex)
        {
            if (string.IsNullOrEmpty(action))
                return currentIndex;

            action = action.Trim();

            if (action.Equals("next", StringComparison.OrdinalIgnoreCase))
            {
                if (currentIndex + 1 < steps.Count)
                    return currentIndex + 1;
                else
                    return 0;
            }

            if (action.Equals("end", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            if (action.StartsWith("Tostep", StringComparison.OrdinalIgnoreCase))
            {
                var parts = action.Split(':');
                if (parts.Length == 2)
                {
                    string target = parts[1].Trim();

                    int index = steps.FindIndex(x => x.Name_Position == target);

                    if (index != -1)
                        return index;
                    else
                    {
                        Console.WriteLine($"Không tìm thấy step: {target}");
                        return 0;
                    }
                }
            }

            return currentIndex;
        }
        private enum Process
        {
            MOVE,
            Wait,
            TRIGGER,
            CLAMPING_CYLINDER,
            RELEASE_CYLINDER,

        }
      
    }
}