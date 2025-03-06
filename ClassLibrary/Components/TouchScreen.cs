namespace DeviceSimulator
{
    public class TouchScreen : Peripheral
    {
        public double SizeInches { get; }
        public bool MultiTouch { get; }

        public TouchScreen(double sizeInches, bool multiTouch) : base("TouchScreen")
        {
            SizeInches = sizeInches;
            MultiTouch = multiTouch;
        }

        public override bool IsCompatibleWith(IDevice device) =>
            device is Smartphone || device is Tablet;
    }
}