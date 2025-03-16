﻿namespace DeviceSimulator
{
    public class ListeningMusicStrategy : IActivityStrategy
    {
        public bool CanExecute(IDevice device)
        {
            return device.HasPeripheral("Speakers") || device.HasPeripheral("Headphones");
        }

        public bool Execute(IDevice device, TimeSpan duration)
        {
            var powerSource = GetPowerSource(device);
            if (powerSource is Battery battery)
            {
                battery.Discharge(duration, BatteryMode.Normal);
                return true;
            }
            return false;
        }

        private PowerSource GetPowerSource(IDevice device)
        {
            var powerStatus = device.CheckPowerStatus();
            if (powerStatus.HasPower)
            {
                var deviceType = device.GetType();
                var field = deviceType.BaseType.GetField("currentPowerSource", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return field?.GetValue(device) as PowerSource;
            }
            return null;
        }
    }
}