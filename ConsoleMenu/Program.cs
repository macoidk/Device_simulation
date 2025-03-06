using System;

namespace DeviceSimulatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Для коректного відображення української мови
            var deviceManager = new DeviceManager();
            var menuHandler = new MenuHandler(deviceManager);
            menuHandler.Run();
        }
    }
}