using Dashboard.Frontend.Stores.States;
using Fluxor;
using Microsoft.AspNetCore.Components;
using System;

namespace Dashboard.Frontend.Components.Core
{
    public partial class ImageViewer : ComponentBase
    {
        [Parameter]
        public string Width { get; set; } = "150px";

        [Parameter]
        public string Height { get; set; } = "150px";

        [Parameter]
        public string HeightSize { get; set; } = "150px";

        [Parameter]
        public string ImgSrc { get; set; }

        [Parameter]
        public int Rotate { get; set; } = 0;

        [Inject]
        private IState<DeviceState> DeviceState { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            DeviceState.StateChanged += DeviceState_StateChanged;
        }

        private void DeviceState_StateChanged(object sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}
