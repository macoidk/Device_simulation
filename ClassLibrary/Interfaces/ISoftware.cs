namespace DeviceSimulator
{
    public interface ISoftware
    {
        string Name { get; }
        IEnumerable<string> Requirements { get; }
        double RequiredStorageGB { get; }
        bool CanRun(IDevice device);
    }
}