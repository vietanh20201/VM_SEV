using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PC_Control_SEV;

namespace Utilities.LogProgram
{
    public static partial class LogProgram 
    {
       
        private static readonly string LogPath = Path.Combine(Main_control.path_LocalFileLog==null ? Environment.CurrentDirectory: Main_control.path_LocalFileLog, "LogProgram");

        private static readonly Queue<Tuple<string, DateTime>> LogQueue = new Queue<Tuple<string, DateTime>>();
        private static readonly Queue<Tuple<string, DateTime>> MesLogQueue = new Queue<Tuple<string, DateTime>>();

        private static readonly object LogLock = new object();
        private static readonly object MesLogLock = new object();

        private static bool isRunning = false;
        private static Thread logThread;

        static LogProgram()
        {
            StartLogThread();
        }

        public static void WriteLog(string log)
        {
            lock (LogLock)
            {
                LogQueue.Enqueue(new Tuple<string, DateTime>(log, DateTime.Now));
            }
        }

        public static void MesWriteLog(string log)
        {
            lock (MesLogLock)
            {
                MesLogQueue.Enqueue(new Tuple<string, DateTime>(log, DateTime.Now));
            }
        }

        private static void StartLogThread()
        {
            if (isRunning) return;

            isRunning = true;
            logThread = new Thread(() =>
            {
                while (isRunning)
                {
                    try
                    {
                        FlushLogs();
                        Thread.Sleep(100);
                    }
                    catch
                    {
                        // Ignore logging error
                    }
                }
            });

            logThread.IsBackground = true;
            logThread.Start();
        }

        private static void FlushLogs()
        {
            List<Tuple<string, DateTime>> logsToWrite = new List<Tuple<string, DateTime>>();
            List<Tuple<string, DateTime>> mesLogsToWrite = new List<Tuple<string, DateTime>>();

            lock (LogLock)
            {
                while (LogQueue.Count > 0)
                {
                    logsToWrite.Add(LogQueue.Dequeue());
                }
            }

            lock (MesLogLock)
            {
                while (MesLogQueue.Count > 0)
                {
                    mesLogsToWrite.Add(MesLogQueue.Dequeue());
                }
            }

            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            foreach (Tuple<string, DateTime> item in logsToWrite)
            {
                string filename = Path.Combine(LogPath, "Log_" + item.Item2.ToString("dd_MM_yyyy") + ".txt");
                File.AppendAllText(filename, "---" + item.Item2.ToString("HH:mm:ss:ff") + " " + item.Item1 + Environment.NewLine);
            }

            foreach (Tuple<string, DateTime> item in mesLogsToWrite)
            {
                string filename = Path.Combine(LogPath, "Mes_" + item.Item2.ToString("dd_MM_yyyy") + ".txt");
                File.AppendAllText(filename, "---" + item.Item2.ToString("HH:mm:ss:ff") + " " + item.Item1 + Environment.NewLine);
            }
        }

        public static void Stop()
        {
            isRunning = false;
            if (logThread != null)
            {
                logThread.Join();
            }
            FlushLogs();
        }
    }
}


