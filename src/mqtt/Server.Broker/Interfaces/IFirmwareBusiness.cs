using Microsoft.AspNetCore.Http;
using Server.Broker.Infrastructure.Entities;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IFirmwareBusiness
    {
        Task<IEnumerable<FirmwareItemResponse>> GetFirmwaresAsync();
        Task<IEnumerable<FirmwareVersionItemResponse>> GetFirmwareVersionsAsync(Guid firmwareId);
        Task<bool> InstallOnDeviceAsync(string deviceName, Guid firmwareId);
        Task<FirmwareVersion> GetFirmwareVersionAsync(Guid id);
        Task<(bool success, bool isConflict)> UploadFileAsync(IFormFile file);
    }
}
