namespace DeviceSimulator
{
    public interface IActivityStrategy
    {
        bool CanExecute(IDevice device);
        bool Execute(IDevice device, TimeSpan duration);
    }
}