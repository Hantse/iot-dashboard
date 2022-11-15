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
    public class DeviceService : IDeviceService
    {
        private readonly HttpClient httpClient;

        public DeviceService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<DeviceItemResponse> GetDeviceAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"device/{id}");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<DeviceItemResponse>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        public async Task<IEnumerable<DeviceLogItemResponse>> GetDeviceLogsAsync(Guid id)
        {
            var response = await httpClient.GetAsync($"device/{id}/logs");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<IEnumerable<DeviceLogItemResponse>>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        public async Task<IEnumerable<DeviceItemResponse>> GetDevicesAsync()
        {
            var response = await httpClient.GetAsync("device");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<IEnumerable<DeviceItemResponse>>(await response.Content.ReadAsStringAsync());
            }

            return null;
        }

        public async Task<bool> SendDeviceMessageAsync(string topic, string message)
        {
            var response = await httpClient.PostAsync($"device/send", new StringContent(JsonSerializer.Serialize(new DeviceMessageRequest()
            {
                Message = message,
                Topic = topic
            }), Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
    }
}
