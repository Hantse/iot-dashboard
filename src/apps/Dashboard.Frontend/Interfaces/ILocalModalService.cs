using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Interfaces
{
    public interface ILocalModalService
    {
        Task<ModalRef> ShowDeployModalAsync(Guid selectedFirmware);
        Task<ModalRef> ShowToolsModal();
    }
}
