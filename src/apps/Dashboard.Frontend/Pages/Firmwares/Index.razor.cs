using AntDesign;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Pages.Firmwares
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public NotificationService Notice { get; set; }

        public async Task OnCompleteCallback(UploadInfo fileinfo)
        {
            if (fileinfo.File.Response.Contains("409"))
            {
                await Notice.Open(new NotificationConfig()
                {
                    Message = "Fail to upload",
                    Description = "This firmware version (checksum) already upload.",
                    NotificationType = NotificationType.Error
                });
            }
            else if (fileinfo.File.State == UploadState.Success)
            {
                await Notice.Open(new NotificationConfig()
                {
                    Message = "Upload success",
                    Description = "This firmware is upload.",
                    NotificationType = NotificationType.Success
                });
            }
        }
    }
}
