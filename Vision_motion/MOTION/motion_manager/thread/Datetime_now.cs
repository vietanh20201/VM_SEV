using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Control_SEV.Motion_Manager.thread
{
    public class DateTimeLabelThread 
    {
        private readonly Label _label;
        private readonly int _intervalMs;

        private Thread _thread;
        private CancellationTokenSource _cts;

        public DateTimeLabelThread(Label label, int intervalMs = 200)
        {
            _label = label ?? throw new ArgumentNullException(nameof(label));
            _intervalMs = Math.Max(10, intervalMs);
        }

        public void Start(string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (_thread != null && _thread.IsAlive) return; // đang chạy rồi

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _thread = new Thread(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    string text = DateTime.Now.ToString(format);
                    try
                    {
                        // Label bị dispose/đóng form -> thoát
                        if (_label.IsDisposed) break;

                        // Cập nhật UI đúng luồng
                        if (_label.InvokeRequired)
                        {
                            _label.BeginInvoke(new Action(() =>
                            {
                                if (!_label.IsDisposed) _label.Text = text;
                            }));
                        }
                        else
                        {
                            _label.Text = text;
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    catch (InvalidOperationException)
                    {
                        // Handle chưa tạo hoặc form đã đóng
                        break;
                    }

                    Thread.Sleep(_intervalMs);
                }
            });

            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Stop()
        {
            try
            {
                if (_cts == null) return;
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
                // Không Join cứng để tránh treo UI; thread là background nên app thoát vẫn ok
                _thread = null;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Error stopping DateTimeLabelThread: {ex.Message}");
            }
            //if (_thread != null)
            //{
            //    if (_thread.IsAlive)
            //    {
            //        _thread.Abort();
            //    }
            //}
        }
    }
}
