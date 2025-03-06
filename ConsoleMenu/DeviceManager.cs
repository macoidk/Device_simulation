using System;
using System.Linq;
using DeviceSimulator;

namespace DeviceSimulatorConsole
{
    public class DeviceManager
    {
        public Laptop Laptop { get; }
        public Smartphone Smartphone { get; }
        public Tablet Tablet { get; }

        public DeviceManager()
        {
            Laptop = new Laptop(3000, 512); // 3000 мАг, 512 ГБ
            Smartphone = new Smartphone(5000, 128); // 5000 мАг, 128 ГБ
            Tablet = new Tablet(7000, 256); // 7000 мАг, 256 ГБ

            var logger = new ConsoleLogger();
            logger.Subscribe(Laptop);
            logger.Subscribe(Smartphone);
            logger.Subscribe(Tablet);
        }

        public IPeripheral GetPeripheralByName(IDevice device, string name)
        {
            var field = typeof(Device).GetField("connectedPeripherals", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var peripherals = field.GetValue(device) as System.Collections.Generic.List<IPeripheral>;
                return peripherals?.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }

        public ISoftware GetSoftwareByName(IDevice device, string name)
        {
            var field = typeof(Device).GetField("installedSoftware", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var softwareList = field.GetValue(device) as System.Collections.Generic.List<ISoftware>;
                return softwareList?.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
            return null;
        }
    }
}