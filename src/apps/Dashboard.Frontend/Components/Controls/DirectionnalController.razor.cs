using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Components.Controls
{
    public partial class DirectionnalController : ComponentBase
    {
        [Parameter]
        public string DeviceName { get; set; }

        public bool KeyUpActivate { get; set; }
        public bool KeyDownActivate { get; set; }
        public bool KeyLeftActivate { get; set; }
        public bool KeyRightActivate { get; set; }

        private ElementReference commandRef;

        string KeyPressed = "";
        string EventInfo = "";

        private double PwmValue = 50;

        [Inject]
        private IDeviceService DeviceService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await commandRef.FocusAsync();
            }
        }

        private async void KeyboardEventHandler(KeyboardEventArgs args)
        {
            var currentValue = false;
            var valueType = args.Type == "keyup" ? false : true;

            var messageService = new BaseCommandModel()
            {
                Command = "MotorMove",
                Value = valueType ? 1 : 0
            };

            switch (args.Key)
            {
                case "ArrowRight":
                    currentValue = KeyRightActivate;
                    KeyRightActivate = valueType;
                    messageService.Bind = "BrakeRight";
                    break;

                case "ArrowLeft":
                    currentValue = KeyLeftActivate;
                    KeyLeftActivate = valueType;
                    messageService.Bind = "BrakeLeft";
                    break;

                case "ArrowUp":
                    currentValue = KeyUpActivate;
                    KeyUpActivate = valueType;
                    messageService.Bind = "Forward";
                    break;

                case "ArrowDown":
                    currentValue = KeyDownActivate;
                    KeyDownActivate = valueType;
                    messageService.Bind = "Backward";
                    break;
            }

            KeyPressed = "Key Pressed is " + args.Key;
            EventInfo = "Event Type " + args.Type;

            if (currentValue != valueType)
            {
                await DeviceService.SendDeviceMessageAsync($"mqtt/device/{DeviceName}/commands/*", JsonSerializer.Serialize(messageService));
            }
        }

        public string GetIcone(string direction)
        {
            if (direction == "ArrowRight")
            {
                return (KeyRightActivate) ? "right-circle" : "right";
            }

            if (direction == "ArrowLeft")
            {
                return (KeyLeftActivate) ? "left-circle" : "left";
            }

            if (direction == "ArrowUp")
            {
                return (KeyUpActivate) ? "up-circle" : "up";
            }

            if (direction == "ArrowDown")
            {
                return (KeyDownActivate) ? "down-circle" : "down";
            }

            return "";
        }


        public string IsFill(string direction)
        {
            if (direction == "ArrowRight")
            {
                return (KeyRightActivate) ? "fill" : "outline";
            }

            if (direction == "ArrowLeft")
            {
                return (KeyLeftActivate) ? "fill" : "outline";
            }

            if (direction == "ArrowUp")
            {
                return (KeyUpActivate) ? "fill" : "outline";
            }

            if (direction == "ArrowDown")
            {
                return (KeyDownActivate) ? "fill" : "outline";
            }

            return "outline";
        }
    }
}
