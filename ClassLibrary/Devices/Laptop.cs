namespace DeviceSimulator
{
    public class Laptop : Device
    {
        private readonly Battery battery;

        public Laptop(int batteryCapacityMAh, double storageGB) : base(storageGB)
        {
            battery = new Battery(batteryCapacityMAh);
            currentPowerSource = battery;
            ConnectPeripheral(new BasicPeripheral("Display"));

            battery.LowPowerWarning += (sender, args) =>
            {
                OnActivityStatusChanged(
                    $"Low battery warning! {args.RemainingPower:F1}% remaining, " +
                    $"approximately {args.RemainingTime.TotalHours:F1} hours left");
            };
        }

        public void ChargeBattery(TimeSpan duration)
        {
            battery.Charge(duration);
            OnActivityStatusChanged($"Charging for {duration.TotalHours:F1} hours");
        }
    }
}