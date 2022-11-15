using Dashboard.Frontend.Interfaces;
using Service.Shared.Contracts.Request;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Services
{
    public class FirmwareService : IFirmwareService
    {
        private readonly HttpClient httpClient;

        public FirmwareService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<FirmwareItemResponse>> GetFirmwaresAsync()
        {
            var response = await httpClient.GetAsync("firmware");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<IEnumerable<FirmwareItemResponse>>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        public async Task<bool> InstallOnDevicesAsync(Guid firmwareId, InstallOnDeviceRequest request)
        {
            var response = await httpClient.PostAsync($"firmware/install/{firmwareId}", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InstallOnDeviceAsync(Guid firmwareId, string deviceName)
        {
            var response = await httpClient.GetAsync($"firmware/install/{deviceName}/{firmwareId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<FirmwareVersionItemResponse>> GetFirmwareVersionsAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"firmware/{id}/versions");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<IEnumerable<FirmwareVersionItemResponse>>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }
    }
}
