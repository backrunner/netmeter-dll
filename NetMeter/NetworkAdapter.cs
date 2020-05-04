using System;
using System.Diagnostics;
using NetMeter.Helper;

namespace NetMeter
{
    public class NetworkAdapter
    {
        private long firstDownloadVlaue, firstUploadValue;
        private long downloadValue, uploadValue;
        private PerformanceCounter downloadSpeedCounter, uploadSpeedCounter;

        public string AdapterName { get; set; }
        public long DownloadSpeed { get; set; }
        public long UploadSpeed { get; set; }
        public double DownloadSpeedKbps { get => this.DownloadSpeed / 1024.0; }
        public double UploadSpeedKbps { get => this.UploadSpeed / 1024.0; }
        public double DownloadSpeedMbps { get => (this.DownloadSpeed / 1024.0) / 1024.0; }
        public double UploadSpeedMbps { get => (this.UploadSpeed / 1024.0) / 1024.0; }
        public string DownloadSpeedFormatted { get => Numeric.SizeFormat(this.DownloadSpeed, "/s"); }
        public string UploadSpeedFormatted { get => Numeric.SizeFormat(this.UploadSpeed, "/s"); }

        public NetworkAdapter(string name)
        {
            if (name == null)
            {
                throw new Exception("Adapter name cannot be null");
            }
            AdapterName = name;
            PerformanceCounterCategory counterCategory = new PerformanceCounterCategory("Network Interface");
            foreach (string interfaceName in counterCategory.GetInstanceNames())
            {   
                if (interfaceName == name)
                {
                    downloadSpeedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
                    uploadSpeedCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
                }
            }
        }

        public void Init()
        {
            UpdateFirstValue();
        }

        public void Refresh()
        {
            downloadValue = downloadSpeedCounter.NextSample().RawValue;
            uploadValue = uploadSpeedCounter.NextSample().RawValue;
            // calc speed
            DownloadSpeed = downloadValue - firstDownloadVlaue;
            UploadSpeed = uploadValue - firstUploadValue;
            UpdateFirstValue();
        }

        public void UpdateFirstValue()
        {
            firstDownloadVlaue = downloadSpeedCounter.NextSample().RawValue;
            firstUploadValue = uploadSpeedCounter.NextSample().RawValue;
        }
    }
}
