using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Interfaces
{
    public interface IDeviceService
    {
        Task<DeviceItemResponse> GetDeviceAsync(Guid id);
        Task<IEnumerable<DeviceLogItemResponse>> GetDeviceLogsAsync(Guid id);
        Task<IEnumerable<DeviceItemResponse>> GetDevicesAsync();
        Task<bool> SendDeviceMessageAsync(string topic, string message);
    }
}
