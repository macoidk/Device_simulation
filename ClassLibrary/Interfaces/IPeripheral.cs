namespace DeviceSimulator
{
    public interface IPeripheral
    {
        string Name { get; }
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        bool IsCompatibleWith(IDevice device);
    }
}