namespace DeviceSimulator
{
    public abstract class PowerSource
    {
        public bool IsAvailable { get; protected set; }
        public abstract double RemainingPower { get; }
        public abstract PowerStatus GetStatus();
    }
}