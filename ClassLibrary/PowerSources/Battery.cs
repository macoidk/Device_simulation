namespace DeviceSimulator
{
    public class Battery : PowerSource
    {
        private readonly int capacityMAh;
        private double currentCapacity;
        private BatteryMode mode;

        public event EventHandler<PowerStatusEventArgs> LowPowerWarning;

        public Battery(int capacityMAh)
        {
            this.capacityMAh = capacityMAh;
            this.currentCapacity = capacityMAh;
            IsAvailable = true;
            mode = BatteryMode.Normal;
        }

        public override double RemainingPower => (currentCapacity / capacityMAh) * 100;

        public TimeSpan GetRemainingTime()
        {
            double hoursRemaining = mode switch
            {
                BatteryMode.Normal when capacityMAh >= 5000 => (currentCapacity / capacityMAh) * 12,
                BatteryMode.Intensive when capacityMAh >= 5000 => (currentCapacity / capacityMAh) * 12,
                BatteryMode.Normal => (currentCapacity / capacityMAh) * 48,
                BatteryMode.Intensive => (currentCapacity / capacityMAh) * 16,
                _ => 0
            };

            return TimeSpan.FromHours(hoursRemaining);
        }

        public void ResetMode()
        {
            mode = BatteryMode.Normal;
        }

        public void Discharge(TimeSpan duration, BatteryMode mode)
        {
            this.mode = mode;

            double dischargePerHour = mode switch
            {
                BatteryMode.Normal when capacityMAh >= 5000 => 100.0 / 12,
                BatteryMode.Intensive when capacityMAh >= 5000 => 100.0 / 4,
                BatteryMode.Normal => 100.0 / 48,
                BatteryMode.Intensive => 100.0 / 16,
                _ => 0
            };

            double totalDischargePercent = dischargePerHour * duration.TotalHours;
            double dischargeMah = (totalDischargePercent / 100.0) * capacityMAh;

            currentCapacity = Math.Max(0, currentCapacity - dischargeMah);

            if (currentCapacity <= capacityMAh * 0.2)
            {
                LowPowerWarning?.Invoke(this, new PowerStatusEventArgs(
                    RemainingPower,
                    GetRemainingTime()
                ));
            }
        }


        public void Charge(TimeSpan duration)
        {
            double chargePerHour = capacityMAh / 2.0; // Повна зарядка за 2 години
            double chargeMah = chargePerHour * duration.TotalHours;
            currentCapacity = Math.Min(capacityMAh, currentCapacity + chargeMah);
        }

        public override PowerStatus GetStatus()
        {
            return new PowerStatus
            {
                HasPower = IsAvailable,
                RemainingPower = RemainingPower,
                RemainingTime = GetRemainingTime()
            };
        }
    }
}