using Service.Shared.Contracts.Request;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Interfaces
{
    public interface IFirmwareService
    {
        Task<IEnumerable<FirmwareItemResponse>> GetFirmwaresAsync();
        Task<IEnumerable<FirmwareVersionItemResponse>> GetFirmwareVersionsAsync(Guid id);
        Task<bool> InstallOnDeviceAsync(Guid firmwareId, string deviceName);
        Task<bool> InstallOnDevicesAsync(Guid firmwareId, InstallOnDeviceRequest request);
    }
}
