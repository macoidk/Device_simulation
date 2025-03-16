using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceSimulator
{
    public abstract class Device : IDevice
    {
        protected bool isOn;
        protected PowerSource currentPowerSource;
        protected readonly List<ISoftware> installedSoftware;
        protected readonly List<IPeripheral> connectedPeripherals;
        protected bool hasNetworkConnection;
        protected double availableStorageGB;

        public event EventHandler<string> ActivityStatusChanged;
        public event EventHandler<PeripheralEventArgs> PeripheralConnected;
        public event EventHandler<PeripheralEventArgs> PeripheralDisconnected;
        

        protected Device(double storageGB)
        {
            installedSoftware = new List<ISoftware>();
            connectedPeripherals = new List<IPeripheral>();
            availableStorageGB = storageGB;
        }

        public bool TurnOn()
        {
            if (currentPowerSource?.IsAvailable == true)
            {
                isOn = true;
                OnActivityStatusChanged("Device turned on");
                return true;
            }
            return false;
        }

        public void TurnOff()
        {
            isOn = false;
            OnActivityStatusChanged("Device turned off");
        }

        public PowerStatus CheckPowerStatus()
        {
            var status = currentPowerSource?.GetStatus() ?? new PowerStatus();
            status.IsOn = isOn;
            return status;
        }

        protected bool CanExecuteActivity(ActivityType activity)
        {
            if (!isOn || currentPowerSource?.IsAvailable != true)
                return false;

            return activity switch
            {
                ActivityType.Gaming or ActivityType.WatchingVideo =>
                    hasNetworkConnection && HasRequiredPeripherals("Display", "Speakers"),
                ActivityType.Chatting =>
                    hasNetworkConnection && HasRequiredPeripherals("Microphone"),
                ActivityType.ListeningMusic =>
                    HasRequiredPeripherals("Speakers"),
                ActivityType.Printing =>
                    HasRequiredPeripherals("Printer"),
                _ => true
            };
        }

        protected bool HasRequiredPeripherals(params string[] requiredPeripherals)
        {
            return requiredPeripherals.All(p =>
                connectedPeripherals.Any(cp => cp.Name.Equals(p, StringComparison.OrdinalIgnoreCase)));
        }

        public bool HasPeripheral(string peripheralName)
        {
            return connectedPeripherals.Any(p =>
                p.Name.Equals(peripheralName, StringComparison.OrdinalIgnoreCase));
        }

        public void ConnectPeripheral(IPeripheral peripheral)
        {
            if (!connectedPeripherals.Contains(peripheral) && peripheral.IsCompatibleWith(this))
            {
                peripheral.Connect();
                connectedPeripherals.Add(peripheral);
                OnActivityStatusChanged($"Connected {peripheral.Name}");
            }
        }

        public void DisconnectPeripheral(IPeripheral peripheral)
        {
            if (connectedPeripherals.Contains(peripheral))
            {
                peripheral.Disconnect();
                connectedPeripherals.Remove(peripheral);
                OnActivityStatusChanged($"Disconnected {peripheral.Name}");
            }
        }

        public void InstallSoftware(ISoftware software, double requiredStorageGB)
        {
            if (availableStorageGB >= requiredStorageGB)
            {
                installedSoftware.Add(software);
                availableStorageGB -= requiredStorageGB;
                OnActivityStatusChanged($"Installed {software.Name}");
            }
            else
            {
                OnActivityStatusChanged("Not enough storage!");
            }
        }

        public void UninstallSoftware(ISoftware software)
        {
            if (installedSoftware.Contains(software))
            {
                installedSoftware.Remove(software);
                availableStorageGB += software.RequiredStorageGB;
                OnActivityStatusChanged($"Uninstalled {software.Name}");
            }
        }

        public void ConnectToNetwork()
        {
            hasNetworkConnection = true;
            OnActivityStatusChanged("Connected to network");
        }

        public void DisconnectFromNetwork()
        {
            hasNetworkConnection = false;
            OnActivityStatusChanged("Disconnected from network");
        }

        public abstract bool ExecuteActivity(ActivityType activity, TimeSpan duration);

        protected virtual bool PerformActivity(ActivityType activity, TimeSpan duration)
        {
            if (!CanExecuteActivity(activity))
            {
                OnActivityStatusChanged($"Cannot execute {activity}: Missing requirements or device is off");
                return false;
            }

            if (currentPowerSource is Battery battery)
            {
                var mode = activity switch
                {
                    ActivityType.Gaming or ActivityType.WatchingVideo => BatteryMode.Intensive,
                    _ => BatteryMode.Normal
                };

                battery.Discharge(duration, mode);
                OnActivityStatusChanged($"Executing {activity} for {duration.TotalHours:F1} hours");
                ResetBatteryMode(); 
                return true;
            }

            OnActivityStatusChanged("No battery available for discharge");
            return false;
        }

        protected void ResetBatteryMode()
        {
            if (currentPowerSource is Battery battery)
            {
                battery.ResetMode();
            }
        }

        protected virtual void OnActivityStatusChanged(string message)
        {
            ActivityStatusChanged?.Invoke(this, message);
        }
        
        protected virtual void OnPeripheralConnected(IPeripheral peripheral)
        {
            PeripheralConnected?.Invoke(this, new PeripheralEventArgs(peripheral));
        }

        protected virtual void OnPeripheralDisconnected(IPeripheral peripheral)
        {
            PeripheralDisconnected?.Invoke(this, new PeripheralEventArgs(peripheral));
        }
        
    }
}