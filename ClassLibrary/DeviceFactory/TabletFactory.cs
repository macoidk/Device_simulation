namespace DeviceSimulator
{
    public class TabletFactory : DeviceFactory
    {
        private readonly int batteryCapacityMAh;
        private readonly double storageGB;

        public TabletFactory(int batteryCapacityMAh, double storageGB)
        {
            this.batteryCapacityMAh = batteryCapacityMAh;
            this.storageGB = storageGB;
        }

        public override IDevice CreateDevice()
        {
            return new Tablet(batteryCapacityMAh, storageGB);
        }
    }
}