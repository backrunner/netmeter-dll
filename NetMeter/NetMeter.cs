using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Threading.Tasks;

namespace NetMeter
{
    public class NetMeter
    {
        private static Dictionary<string, NetworkAdapter> adapters = new Dictionary<string, NetworkAdapter>();
        private static Dictionary<string, SpeedMonitor> monitors = new Dictionary<string, SpeedMonitor>();

        public async Task<object> Init(object o)
        {
            string name = (string)o;
            return await Task.Run(() => InitAdapter(name));
        }

        public async Task<object> GetAdapterNames(object o)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(GetNames()));
        }

        public async Task<object> GetActivatedAdapter(object o)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(GetActivatedAdapterName()));
        }

        public async Task<object> Start(object o)
        {
            string name = (string)o;
            return await Task.Run(() => StartMonitor(name));
        }

        public async Task<object> Stop(object o)
        {
            string name = (string)o;
            return await Task.Run(() => StopMonitor(name));
        }

        public async Task<object> GetDownloadSpeed(object o)
        {
            string name = (string)o;
            return await Task.Run(() => adapters[name].DownloadSpeedFormatted);
        }

        public async Task<object> GetUploadSpeed(object o)
        {
            string name = (string)o;
            return await Task.Run(() => adapters[name].UploadSpeedFormatted);
        }

        public async Task<object> GetDownloadSpeedMbps(object o)
        {
            string name = (string)o;
            return await Task.Run(() => adapters[name].DownloadSpeedMbps);
        }

        public async Task<object> GetUploadSpeedMbps(object o)
        {
            string name = (string)o;
            return await Task.Run(() => adapters[name].UploadSpeedMbps);
        }

        public async Task<object> GetDownloadSpeedKbps(object o)
        {
            string name = (string)o;
            return await Task.Run(() => adapters[name].DownloadSpeedKbps);
        }

        public async Task<object> GetUploadSpeedKbps(object o)
        {
            string name = (string)o;
            return await Task.Run(() => adapters[name].UploadSpeedKbps);
        }

        private bool InitAdapter(string name)
        {
            NetworkAdapter adapter = new NetworkAdapter(name);
            SpeedMonitor monitor = new SpeedMonitor(adapter);
            monitor.Start();
            // Push into dictonary
            adapters.Add(name, adapter);
            monitors.Add(name, monitor);
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

        public string GetActivatedAdapterName()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["IPEnabled"].ToString() == "True")
                {
                    return mo["Caption"].ToString();
                }
            }
            return null;
        }

        private bool StartMonitor(string name)
        {
            monitors[name].Start();
            return true;
        }

        private bool StopMonitor(string name)
        {
            monitors[name].Stop();
            return true;
        }
    }
}
