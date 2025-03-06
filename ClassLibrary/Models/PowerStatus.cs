namespace DeviceSimulator
{
    public class PowerStatus
    {
        public bool IsOn { get; set; }
        public bool HasPower { get; set; }
        public double RemainingPower { get; set; }
        public TimeSpan? RemainingTime { get; set; }
    }
}