using Dashboard.Frontend.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public IRealtimeService RealtimeService { get; set; }

        [Inject]
        public ILocalModalService LocalModalService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RealtimeService.StartListen();
        }
    }
}
