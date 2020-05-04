using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetMeter
{
    public class NetMeter
    {
        private NetworkAdapter adapter;
        private SpeedMonitor monitor;

        public async Task<object> Init(object o)
        {
            string name = (string)o;
            return await Task.Run(() => InitAdapter(name));
        }

        public async Task<object> GetAdapterNames(object o)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(GetNames()));
        }

        public async Task<object> Start(object o)
        {
            return await Task.Run(() => StartMonitor());
        }

        public async Task<object> Stop(object o)
        {
            return await Task.Run(() => StopMonitor());
        }

        public async Task<object> GetDownloadSpeed(object o)
        {
            return await Task.Run(() => adapter.DownloadSpeedFormatted);
        }

        public async Task<object> GetUploadSpeed(object o)
        {
            return await Task.Run(() => adapter.UploadSpeedFormatted);
        }

        public async Task<object> GetDownloadSpeedMbps(object o)
        {
            return await Task.Run(() => adapter.DownloadSpeedMbps);
        }

        public async Task<object> GetUploadSpeedMbps(object o)
        {
            return await Task.Run(() => adapter.UploadSpeedMbps);
        }

        public async Task<object> GetDownloadSpeedKbps(object o)
        {
            return await Task.Run(() => adapter.DownloadSpeedKbps);
        }

        public async Task<object> GetUploadSpeedKbps(object e)
        {
            return await Task.Run(() => adapter.UploadSpeedKbps);
        }

        private bool InitAdapter(string name)
        {
            adapter = new NetworkAdapter(name);
            monitor = new SpeedMonitor(adapter);
            monitor.Start();
            return true;
        }

        private ArrayList GetNames()
        {
            PerformanceCounterCategory counterCategory = new PerformanceCounterCategory("Network Interface");
            ArrayList adapters = new ArrayList();
            foreach (string interfaceName in counterCategory.GetInstanceNames())
            {
                adapters.Add(interfaceName);
            }
            return adapters;
        }

        private bool StartMonitor()
        {
            monitor.Start();
            return true;
        }

        private bool StopMonitor()
        {
            monitor.Stop();
            return true;
        }
    }
}
