namespace DeviceSimulator
{
    public class MainsPower : PowerSource
    {
        public MainsPower()
        {
            IsAvailable = true;
        }

        public override double RemainingPower => IsAvailable ? 100 : 0;

        public override PowerStatus GetStatus()
        {
            return new PowerStatus
            {
                HasPower = IsAvailable,
                RemainingPower = RemainingPower,
                RemainingTime = null
            };
        }
    }
}