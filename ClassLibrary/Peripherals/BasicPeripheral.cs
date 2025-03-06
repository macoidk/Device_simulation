namespace DeviceSimulator
{
    public class BasicPeripheral : Peripheral
    {
        public BasicPeripheral(string name) : base(name) { }
        public override bool IsCompatibleWith(IDevice device) => true;
    }
}