namespace DeviceSimulator
{
    public class Software : ISoftware
    {
        public string Name { get; }
        public IEnumerable<string> Requirements { get; }
        public double RequiredStorageGB { get; }
        public bool RequiresNetwork { get; }

        public Software(string name, double requiredStorageGB, bool requiresNetwork, params string[] requirements)
        {
            Name = name;
            RequiredStorageGB = requiredStorageGB;
            RequiresNetwork = requiresNetwork;
            Requirements = requirements;
        }

        public bool CanRun(IDevice device)
        {
            var status = device.CheckPowerStatus();
            return status.IsOn && Requirements.All(r =>
                device.HasPeripheral(r));
        }
    }
}