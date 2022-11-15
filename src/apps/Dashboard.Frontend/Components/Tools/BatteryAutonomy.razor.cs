using Microsoft.AspNetCore.Components;
using System;

namespace Dashboard.Frontend.Components.Tools
{
    public partial class BatteryAutonomy : ComponentBase
    {
        public double Capacity { get; set; } = 4200;
        public double Consumption { get; set; } = 400;
        public TimeSpan? Result { get; set; } = null;

        public void CalculateDuration()
        {
            var durationValue = (double)Capacity / (double)Consumption;
            Result = TimeSpan.FromHours(durationValue);
        }

        private string FormatMah(double value)
        {
            Capacity = value;
            return $"{value.ToString("n0")}mah";
        }

        private string ParseMah(string value)
        {
            return value.Replace("mah", "");
        }

        private string FormatConsumptionMah(double value)
        {
            Consumption = value;
            return $"{value.ToString("n0")}mah";
        }

        private string ParseConsumptionMah(string value)
        {
            return value.Replace("mah", "");
        }
    }
}
