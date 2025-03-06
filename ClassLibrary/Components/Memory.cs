namespace DeviceSimulator
{
    public abstract class Memory : Peripheral
    {
        public int CapacityGB { get; }
        public double SpeedMHz { get; }

        protected Memory(string name, int capacityGB, double speedMHz) : base(name)
        {
            CapacityGB = capacityGB;
            SpeedMHz = speedMHz;
        }

        public override bool IsCompatibleWith(IDevice device) => true;
    }

    public class RAM : Memory
    {
        public RAM(int capacityGB, double speedMHz) : base("RAM", capacityGB, speedMHz) { }
    }

    public class SSD : Memory
    {
        public SSD(int capacityGB, double speedMHz) : base("SSD", capacityGB, speedMHz) { }
    }

    public class HDD : Memory
    {
        public HDD(int capacityGB, double speedMHz) : base("HDD", capacityGB, speedMHz) { }
    }
}