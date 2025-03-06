namespace DeviceSimulator
{
    public interface IDevice
    {
        bool TurnOn();
        void TurnOff();
        PowerStatus CheckPowerStatus();
        bool ExecuteActivity(ActivityType activity, TimeSpan duration);
        bool HasPeripheral(string peripheralName);
        void ConnectPeripheral(IPeripheral peripheral);
        void DisconnectPeripheral(IPeripheral peripheral);
        void InstallSoftware(ISoftware software, double requiredStorageGB); 
        void UninstallSoftware(ISoftware software);
        void ConnectToNetwork();
        void DisconnectFromNetwork();

        event EventHandler<string> ActivityStatusChanged;
    }
}