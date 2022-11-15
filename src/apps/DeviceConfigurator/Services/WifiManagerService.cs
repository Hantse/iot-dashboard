using DeviceConfigurator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConfigurator.Services
{
    public partial class WifiManagerService : IWifiManagerService
    {
        public partial Task<IEnumerable<string>> GetAvailableNetworksAsync();

    }
}
