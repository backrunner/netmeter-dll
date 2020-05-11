using System;
using System.Diagnostics;
using System.Timers;

namespace NetMeter
{
    public class SpeedMonitor
    {
        private NetworkAdapter adapter;
        private Timer timer;
        
        public SpeedMonitor(NetworkAdapter a)
        {
            adapter = a;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            adapter.Refresh();
        }

        public void Start()
        {
            adapter.Init();
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }
    }
}
