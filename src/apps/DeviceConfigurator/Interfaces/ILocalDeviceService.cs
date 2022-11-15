using DeviceConfigurator.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConfigurator.Interfaces
{
    public interface  ILocalDeviceService
    {
        Task<(bool find, string name)> ScanDeviceAsync();
        Task<bool> ConfigureDevice(string ssid, string password);
        Task<AvailableNetworkScanResponse> ScanNetworks();
    }
}
