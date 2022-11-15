using DeviceConfigurator.Contracts.Response;
using DeviceConfigurator.Interfaces;
using System.Text.Json;

namespace DeviceConfigurator.Services
{
    public class LocalDeviceService : ILocalDeviceService
    {
        public async Task<bool> ConfigureDevice(string ssid, string password)
        {
            try
            {
                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("http://192.168.4.1")
                };

                var httpResponse = await httpClient.GetAsync($"configure?ssid={ssid}&password={password}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = JsonSerializer.Deserialize<ConfigureResponse>(await httpResponse.Content.ReadAsStringAsync());
                    return response != null && response.Success;

                }
            }
            catch (Exception e)
            {

            }

            return false;
        }

        public async Task<(bool find, string name)> ScanDeviceAsync()
        {
            try
            {
                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("http://192.168.4.1")
                };

                var httpResponse = await httpClient.GetAsync(string.Empty);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = JsonSerializer.Deserialize<LocalDevicePing>(await httpResponse.Content.ReadAsStringAsync());
                    return (true, response.Hostname);

                }
            }
            catch (Exception e)
            {

            }

            return (false, string.Empty);
        }

        public async Task<AvailableNetworkScanResponse> ScanNetworks()
        {
            try
            {
                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri("http://192.168.4.1")
                };

                var httpResponse = await httpClient.GetAsync($"scan");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadAsStringAsync();
                    var networks = JsonSerializer.Deserialize<AvailableNetworkScanResponse>(response);
                    return networks;

                }
            }
            catch (Exception e)
            {

            }

            return null;
        }
    }
}
