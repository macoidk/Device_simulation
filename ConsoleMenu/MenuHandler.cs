using System;
using DeviceSimulator;

namespace DeviceSimulatorConsole
{
    public class MenuHandler
    {
        private readonly DeviceManager _deviceManager;
        private IDevice _currentDevice;

        public MenuHandler(DeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            _currentDevice = null;
        }

        public void Run()
        {
            while (true)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SelectDevice();
                        break;
                    case "2":
                        if (_currentDevice != null) ShowDeviceMenu();
                        else Console.WriteLine("Спочатку оберіть пристрій!");
                        break;
                    case "3":
                        Console.WriteLine("До зустрічі!");
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                        break;
                }
            }
        }

        private void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Головне меню ===");
            Console.WriteLine("1. Обрати пристрій");
            Console.WriteLine("2. Керувати поточним пристроєм");
            Console.WriteLine("3. Вийти");
            Console.Write("Ваш вибір: ");
        }

        private void SelectDevice()
        {
            Console.Clear();
            Console.WriteLine("=== Вибір пристрою ===");
            Console.WriteLine("Доступні пристрої:");
            Console.WriteLine("1. Ноутбук");
            Console.WriteLine("2. Смартфон");
            Console.WriteLine("3. Планшет");
            Console.Write("Виберіть пристрій (1-3): ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    _currentDevice = _deviceManager.Laptop;
                    Console.WriteLine("Обрано ноутбук.");
                    break;
                case "2":
                    _currentDevice = _deviceManager.Smartphone;
                    Console.WriteLine("Обрано смартфон.");
                    break;
                case "3":
                    _currentDevice = _deviceManager.Tablet;
                    Console.WriteLine("Обрано планшет.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }
            Console.WriteLine("Натисніть Enter, щоб продовжити...");
            Console.ReadLine();
        }

        private void ShowDeviceMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== Керування пристроєм: {_currentDevice.GetType().Name} ===");
                var status = _currentDevice.CheckPowerStatus();
                Console.WriteLine($"Статус: {(status.IsOn ? "Увімкнено" : "Вимкнено")}, " +
                                  $"Залишок заряду: {status.RemainingPower:F1}%, " +
                                  $"Час роботи: {(status.RemainingTime.HasValue ? $"{status.RemainingTime.Value.TotalHours:F1} год" : "N/A")}");
                Console.WriteLine("1. Увімкнути пристрій");
                Console.WriteLine("2. Вимкнути пристрій");
                Console.WriteLine("3. Виконати активність");
                Console.WriteLine("4. Підключити периферійний пристрій");
                Console.WriteLine("5. Відключити периферійний пристрій");
                Console.WriteLine("6. Встановити програмне забезпечення");
                Console.WriteLine("7. Видалити програмне забезпечення");
                Console.WriteLine("8. Підключитися до мережі");
                Console.WriteLine("9. Відключитися від мережі");
                Console.WriteLine("10. Зарядити батарею");
                Console.WriteLine("11. Повернутися до головного меню");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        _currentDevice.TurnOn();
                        break;
                    case "2":
                        _currentDevice.TurnOff();
                        break;
                    case "3":
                        ExecuteActivity();
                        break;
                    case "4":
                        ConnectPeripheral();
                        break;
                    case "5":
                        DisconnectPeripheral();
                        break;
                    case "6":
                        InstallSoftware();
                        break;
                    case "7":
                        UninstallSoftware();
                        break;
                    case "8":
                        _currentDevice.ConnectToNetwork();
                        break;
                    case "9":
                        _currentDevice.DisconnectFromNetwork();
                        break;
                    case "10":
                        ChargeBattery();
                        break;
                    case "11":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір!");
                        break;
                }
                Console.WriteLine("Натисніть Enter, щоб продовжити...");
                Console.ReadLine();
            }
        }

        private void ExecuteActivity()
        {
            Console.WriteLine("Оберіть активність:");
            Console.WriteLine("1. Грати в ігри");
            Console.WriteLine("2. Працювати");
            Console.WriteLine("3. Чатитися");
            Console.WriteLine("4. Дивитися відео");
            Console.WriteLine("5. Слухати музику");
            Console.WriteLine("6. Друкувати");
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            ActivityType activity = choice switch
            {
                "1" => ActivityType.Gaming,
                "2" => ActivityType.Working,
                "3" => ActivityType.Chatting,
                "4" => ActivityType.WatchingVideo,
                "5" => ActivityType.ListeningMusic,
                "6" => ActivityType.Printing,
                _ => throw new ArgumentException("Невірна активність")
            };

            Console.Write("Введіть тривалість (години): ");
            if (double.TryParse(Console.ReadLine(), out double hours))
            {
                _currentDevice.ExecuteActivity(activity, TimeSpan.FromHours(hours));
            }
            else
            {
                Console.WriteLine("Невірний формат часу!");
            }
        }

        private void ConnectPeripheral()
        {
            Console.WriteLine("Оберіть периферійний пристрій:");
            Console.WriteLine("1. Принтер");
            Console.WriteLine("2. Динаміки");
            Console.WriteLine("3. Навушники");
            Console.WriteLine("4. Мікрофон");
            Console.WriteLine("5. Дисплей"); 
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            IPeripheral peripheral = choice switch
            {
                "1" => new Printer(),
                "2" => new Speakers(),
                "3" => new Headphones(),
                "4" => new Microphone(),
                "5" => new BasicPeripheral("Display"), 
                _ => null
            };

            if (peripheral != null)
            {
                _currentDevice.ConnectPeripheral(peripheral);
            }
            else
            {
                Console.WriteLine("Невірний вибір!");
            }
        }

        private void DisconnectPeripheral()
        {
            Console.WriteLine("Введіть назву периферійного пристрою для відключення (Printer, Speakers, Headphones, Microphone): ");
            string name = Console.ReadLine();
            var peripheral = _deviceManager.GetPeripheralByName(_currentDevice, name);
            if (peripheral != null)
            {
                _currentDevice.DisconnectPeripheral(peripheral);
            }
            else
            {
                Console.WriteLine("Пристрій не знайдено або не підключено!");
            }
        }

        private void InstallSoftware()
        {
            Console.WriteLine("Введіть назву програми: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введіть розмір програми (ГБ): ");
            if (double.TryParse(Console.ReadLine(), out double size))
            {
                var software = new Software(name, size, requiresNetwork: false);
                _currentDevice.InstallSoftware(software, size);
            }
            else
            {
                Console.WriteLine("Невірний формат розміру!");
            }
        }

        private void UninstallSoftware()
        {
            Console.WriteLine("Введіть назву програми для видалення: ");
            string name = Console.ReadLine();
            var software = _deviceManager.GetSoftwareByName(_currentDevice, name);
            if (software != null)
            {
                _currentDevice.UninstallSoftware(software);
            }
            else
            {
                Console.WriteLine("Програму не знайдено!");
            }
        }

        private void ChargeBattery()
        {
            Console.Write("Введіть час зарядки (години): ");
            if (double.TryParse(Console.ReadLine(), out double hours))
            {
                switch (_currentDevice)
                {
                    case Laptop laptop:
                        laptop.ChargeBattery(TimeSpan.FromHours(hours));
                        break;
                    case Smartphone smartphone:
                        smartphone.ChargeBattery(TimeSpan.FromHours(hours));
                        break;
                    case Tablet tablet:
                        tablet.ChargeBattery(TimeSpan.FromHours(hours));
                        break;
                }
            }
            else
            {
                Console.WriteLine("Невірний формат часу!");
            }
        }
    }
}