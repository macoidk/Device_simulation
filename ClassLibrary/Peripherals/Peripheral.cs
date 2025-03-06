// SOLID: Single Responsibility Principle - базовий клас для периферійних пристроїв
namespace DeviceSimulator
{
    public abstract class Peripheral : IPeripheral
    {
        public string Name { get; }
        public bool IsConnected { get; private set; }

        protected Peripheral(string name)
        {
            Name = name;
        }

        public void Connect() => IsConnected = true;
        public void Disconnect() => IsConnected = false;

        public abstract bool IsCompatibleWith(IDevice device);
    }
}