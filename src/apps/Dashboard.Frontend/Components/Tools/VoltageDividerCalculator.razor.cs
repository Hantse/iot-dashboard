using Microsoft.AspNetCore.Components;

namespace Dashboard.Frontend.Components.Tools
{
    public partial class VoltageDividerCalculator : ComponentBase
    {
        public double Vs { get; set; } = 11.1;
        public double R1 { get; set; } = 100;
        public double R2 { get; set; } = 47;

        public double GetVoltage()
        {
            return (Vs * R2) / (R1 + R2);
        }
    }
}
