namespace DeviceSimulator
{
    public class Processor : Peripheral
    {
        public double ClockSpeedGHz { get; }
        public int Cores { get; }

        public Processor(string name, double clockSpeedGHz, int cores) : base(name)
        {
            ClockSpeedGHz = clockSpeedGHz;
            Cores = cores;
        }

        public override bool IsCompatibleWith(IDevice device) => true;
    }
}