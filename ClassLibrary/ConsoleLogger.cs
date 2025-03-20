using System;

namespace DeviceSimulator
{
    public class ConsoleLogger
    {
        public void Subscribe(IDevice device)
        {
            device.ActivityStatusChanged += (sender, message) =>
            {
                Console.WriteLine(message);
            };
            
            device.PeripheralConnected += (sender, args) =>
            {
                Console.WriteLine($"Connected {args.Peripheral.Name}");
            };
            
            device.PeripheralDisconnected += (sender, args) =>
            {
                Console.WriteLine($"Disconnected {args.Peripheral.Name}");
            };
        }
    }
}