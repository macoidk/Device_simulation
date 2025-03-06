using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace DeviceSimulator
{
    public class Tablet : Device
    {
        private readonly Battery battery;
        private readonly TouchScreen touchScreen;

        public Tablet(int batteryCapacityMAh, double storageGB) : base(storageGB)
        {
            battery = new Battery(batteryCapacityMAh);
            currentPowerSource = battery;
            touchScreen = new TouchScreen(10.1, true);

            ConnectPeripheral(touchScreen);
            ConnectPeripheral(new Processor("Tablet CPU", 2.4, 4));
            ConnectPeripheral(new RAM(4, 2133));
            ConnectPeripheral(new SSD(128, 500));

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