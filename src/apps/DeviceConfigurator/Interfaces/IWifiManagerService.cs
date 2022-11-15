using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConfigurator.Interfaces
{
    public interface IWifiManagerService
    {
        Task<IEnumerable<string>> GetAvailableNetworksAsync();
    }
}
