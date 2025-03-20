namespace DeviceSimulator
{
    public class PeripheralEventArgs : EventArgs
    {
        public IPeripheral Peripheral { get; }

        public PeripheralEventArgs(IPeripheral peripheral)
        {
            Peripheral = peripheral;
        }
    }
}