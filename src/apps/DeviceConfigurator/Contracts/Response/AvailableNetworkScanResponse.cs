using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeviceConfigurator.Contracts.Response
{
    public class AvailableNetworkScanResponse
    {
        [JsonPropertyName("availableNetworks")]
        public List<AvailableNetworkResponse> AvailableNetworks { get; set; }
    }
}
