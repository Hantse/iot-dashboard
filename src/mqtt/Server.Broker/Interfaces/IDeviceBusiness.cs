using Microsoft.AspNetCore.Http;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IDeviceBusiness
    {
        Task<DeviceItemResponse[]> GetDevicesAsync();
        Task<DeviceLogItemResponse[]> GetDeviceLog(Guid id);
        Task<bool> UploadFileAsync(string deviceId, IFormFile file);
        Task<bool> UpdateDeviceIpAsync(string deviceName, string ip);
        Task<bool> UpdateDeviceBoardInformationsAsync(string deviceName, string boardInformations);
    }
}
