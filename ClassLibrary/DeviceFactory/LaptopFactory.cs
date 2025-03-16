namespace DeviceSimulator
{
    public class LaptopFactory : DeviceFactory
    {
        private readonly int batteryCapacityMAh;
        private readonly double storageGB;

        public LaptopFactory(int batteryCapacityMAh, double storageGB)
        {
            this.batteryCapacityMAh = batteryCapacityMAh;
            this.storageGB = storageGB;
        }

        public override IDevice CreateDevice()
        {
            return new Laptop(batteryCapacityMAh, storageGB);
        }
    }
}