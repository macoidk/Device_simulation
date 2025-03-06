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

        public override bool ExecuteActivity(ActivityType activity, TimeSpan duration)
        {
            if (!CanExecuteActivity(activity))
            {
                OnActivityStatusChanged($"Cannot execute {activity}: Missing requirements or device is off");
                return false;
            }

            var mode = activity switch
            {
                ActivityType.Gaming or ActivityType.WatchingVideo => BatteryMode.Intensive,
                _ => BatteryMode.Normal
            };

            battery.Discharge(duration, mode);
            OnActivityStatusChanged($"Executing {activity} for {duration.TotalHours:F1} hours");
            return true;
        }

        public void ChargeBattery(TimeSpan duration)
        {
            battery.Charge(duration);
            OnActivityStatusChanged($"Charging for {duration.TotalHours:F1} hours");
        }
    }
}