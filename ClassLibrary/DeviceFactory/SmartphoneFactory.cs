namespace DeviceSimulator
{
    public class SmartphoneFactory : DeviceFactory
    {
        private readonly int batteryCapacityMAh;
        private readonly double storageGB;

        public SmartphoneFactory(int batteryCapacityMAh, double storageGB)
        {
            this.batteryCapacityMAh = batteryCapacityMAh;
            this.storageGB = storageGB;
        }

        public override IDevice CreateDevice()
        {
            return new Smartphone(batteryCapacityMAh, storageGB);
        }
    }
}