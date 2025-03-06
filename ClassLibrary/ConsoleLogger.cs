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
        }
    }
}