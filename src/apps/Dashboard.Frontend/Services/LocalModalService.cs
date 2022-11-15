using AntDesign;
using Dashboard.Frontend.Components.Firmware;
using Dashboard.Frontend.Components.Tools;
using Dashboard.Frontend.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Services
{
    public class LocalModalService : ILocalModalService
    {
        private readonly ModalService modalService;

        public LocalModalService(ModalService modalService)
        {
            this.modalService = modalService;
        }

        public Task<ModalRef> ShowDeployModalAsync(Guid selectedFirmware)
        {
            var deployModalContent = new RenderFragment(builder =>
            {
                builder.OpenComponent<DeployModal>(0);
                builder.AddAttribute(0, "SelectedFirmwareId", selectedFirmware);
                builder.CloseComponent();
            });

            var options = new ModalOptions()
            {
                Width = 420,
                MaskClosable = true,
                Footer = null,
                Content = deployModalContent,
            };

            return modalService.CreateModalAsync(options);
        }

        public Task<ModalRef> ShowToolsModal()
        {
            var deployModalContent = new RenderFragment(builder =>
            {
                builder.OpenComponent<ToolsModal>(0);
                builder.CloseComponent();
            });

            var options = new ModalOptions()
            {
                Width = 420,
                MaskClosable = true,
                Footer = null,
                Content = deployModalContent,
            };

            return modalService.CreateModalAsync(options);
        }
    }
}
