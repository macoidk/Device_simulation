using System;
using System.Linq;
using DeviceSimulator;

namespace DeviceSimulatorConsole
{
    public class DeviceManager
    {
        public IDevice Laptop { get; }
        public IDevice Smartphone { get; }
        public IDevice Tablet { get; }

        public DeviceManager()
        {
            var laptopFactory = new LaptopFactory(3000, 512);
            Laptop = laptopFactory.CreateDevice();

            var smartphoneFactory = new SmartphoneFactory(5000, 128);
            Smartphone = smartphoneFactory.CreateDevice();

            var tabletFactory = new TabletFactory(7000, 256);
            Tablet = tabletFactory.CreateDevice();
            
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