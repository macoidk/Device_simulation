namespace DeviceSimulator
{
    public class PowerStatusEventArgs : EventArgs
    {
        public double RemainingPower { get; }
        public TimeSpan RemainingTime { get; }

        public PowerStatusEventArgs(double remainingPower, TimeSpan remainingTime)
        {
            RemainingPower = remainingPower;
            RemainingTime = remainingTime;
        }
    }
}