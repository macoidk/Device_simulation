using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Демонстрацiя роботи системи ===\n");

        // Створення пристроїв з рiзною ємнiстю акумуляторiв
        var laptop = new Laptop(5000);      // 12 год звичайної роботи / 4 год iнтенсивної
        var smartphone = new Smartphone(3000); // 48 год звичайної роботи / 16 год iнтенсивної
        var tablet = new Tablet(7000);      // 12 год звичайної роботи / 4 год iнтенсивної

        // Пiдписка на подiї для вiдслiдковування статусу
        laptop.ActivityStatusChanged += (s, m) => Console.WriteLine($"Laptop: {m}");
        smartphone.ActivityStatusChanged += (s, m) => Console.WriteLine($"Smartphone: {m}");
        tablet.ActivityStatusChanged += (s, m) => Console.WriteLine($"Tablet: {m}");

        // Створення всiх можливих периферiйних пристроїв
        var printer = new BasicPeripheral("Printer");
        var speakers = new BasicPeripheral("Speakers");
        var microphone = new BasicPeripheral("Microphone");
        var display = new BasicPeripheral("Display");

        // Створення рiзного програмного забезпечення з рiзними вимогами
        var gameApp = new Software("Game", true, "Display", "Speakers");
        var chatApp = new Software("Chat", true, "Microphone");
        var musicApp = new Software("Music Player", false, "Speakers");
        var videoApp = new Software("Video Player", true, "Display", "Speakers");
        var workApp = new Software("Work Application", false, "Display");
        var printApp = new Software("Print Application", false, "Printer");

        Console.WriteLine("\n=== Тестування ноутбука ===");

        // Базове налаштування ноутбука
        laptop.TurnOn();
        laptop.ConnectToNetwork();

        Console.WriteLine("\n--- Тест 1: Неповне пiдключення периферiї ---");
        laptop.InstallSoftware(gameApp);
        // Не повинно працювати - вiдсутнi display та speakers
        laptop.ExecuteActivity(ActivityType.Gaming, TimeSpan.FromHours(1));

        Console.WriteLine("\n--- Тест 2: Часткове пiдключення периферiї ---");
        laptop.ConnectPeripheral(display);
        // Не повинно працювати - вiдсутнi speakers
        laptop.ExecuteActivity(ActivityType.Gaming, TimeSpan.FromHours(1));

        Console.WriteLine("\n--- Тест 3: Повне пiдключення периферiї ---");
        laptop.ConnectPeripheral(speakers);
        laptop.ConnectPeripheral(microphone);
        laptop.ConnectPeripheral(printer);

        // Встановлення всього ПЗ
        laptop.InstallSoftware(chatApp);
        laptop.InstallSoftware(musicApp);
        laptop.InstallSoftware(videoApp);
        laptop.InstallSoftware(workApp);
        laptop.InstallSoftware(printApp);

        Console.WriteLine("\n--- Тест 4: Всi активностi з повним налаштуванням ---");
        // Має працювати - всi вимоги виконанi
        laptop.ExecuteActivity(ActivityType.Gaming, TimeSpan.FromHours(1));
        // Має працювати - є мiкрофон та мережа
        laptop.ExecuteActivity(ActivityType.Chatting, TimeSpan.FromHours(1));
        // Має працювати - є speakers
        laptop.ExecuteActivity(ActivityType.ListeningMusic, TimeSpan.FromHours(1));
        // Має працювати - є display та speakers
        laptop.ExecuteActivity(ActivityType.WatchingVideo, TimeSpan.FromHours(1));
        // Має працювати - є display
        laptop.ExecuteActivity(ActivityType.Working, TimeSpan.FromHours(1));
        // Має працювати - є printer
        laptop.ExecuteActivity(ActivityType.Printing, TimeSpan.FromHours(1));

        Console.WriteLine("\n--- Тест 5: Робота без мережi ---");
        laptop.DisconnectFromNetwork();
        // Не повинно працювати - потрiбна мережа
        laptop.ExecuteActivity(ActivityType.Gaming, TimeSpan.FromHours(1));
        // Не повинно працювати - потрiбна мережа
        laptop.ExecuteActivity(ActivityType.Chatting, TimeSpan.FromHours(1));
        // Має працювати - не потребує мережi
        laptop.ExecuteActivity(ActivityType.ListeningMusic, TimeSpan.FromHours(1));
        // Не повинно працювати - потрiбна мережа
        laptop.ExecuteActivity(ActivityType.WatchingVideo, TimeSpan.FromHours(1));
        // Має працювати - не потребує мережi
        laptop.ExecuteActivity(ActivityType.Working, TimeSpan.FromHours(1));
        // Має працювати - не потребує мережi
        laptop.ExecuteActivity(ActivityType.Printing, TimeSpan.FromHours(1));

        Console.WriteLine("\n=== Тестування планшета ===");
        tablet.TurnOn();
        tablet.ConnectToNetwork();

        Console.WriteLine("\n--- Тест 6: Специфiчнi можливостi планшета ---");
        // Має працювати - планшет має вбудований display
        tablet.ConnectPeripheral(speakers);
        tablet.InstallSoftware(videoApp);
        tablet.InstallSoftware(musicApp);
        tablet.InstallSoftware(gameApp);

        // Має працювати - є всi необхiднi компоненти
        tablet.ExecuteActivity(ActivityType.WatchingVideo, TimeSpan.FromHours(1));
        // Має працювати - є speakers
        tablet.ExecuteActivity(ActivityType.ListeningMusic, TimeSpan.FromHours(1));
        // Має працювати - є display та speakers
        tablet.ExecuteActivity(ActivityType.Gaming, TimeSpan.FromHours(1));

        var status = tablet.CheckPowerStatus();
        Console.WriteLine($"Планшет - залишок заряду: {status.RemainingPower:F1}%");

        Console.WriteLine("\n=== Тестування смартфона ===");
        smartphone.TurnOn();
        smartphone.ConnectToNetwork();

        Console.WriteLine("\n--- Тест 7: Специфiчнi можливостi смартфона ---");
        smartphone.ConnectPeripheral(speakers);
        smartphone.ConnectPeripheral(microphone);
        smartphone.InstallSoftware(chatApp);
        smartphone.InstallSoftware(musicApp);
        smartphone.InstallSoftware(videoApp);

        // Має працювати - є мiкрофон та мережа
        smartphone.ExecuteActivity(ActivityType.Chatting, TimeSpan.FromHours(1));
        // Має працювати - є speakers
        smartphone.ExecuteActivity(ActivityType.ListeningMusic, TimeSpan.FromHours(1));
        // Має працювати - смартфон має вбудований display
        smartphone.ExecuteActivity(ActivityType.WatchingVideo, TimeSpan.FromHours(1));

        Console.WriteLine("\n--- Тест 8: Тест розряду батареї ---");
        // Інтенсивне використання для швидкого розряду
        smartphone.ExecuteActivity(ActivityType.Gaming, TimeSpan.FromHours(8));
        status = smartphone.CheckPowerStatus();
        Console.WriteLine($"Смартфон - залишок заряду пiсля iнтенсивного використання: {status.RemainingPower:F1}%");

        // Вимкнення всiх пристроїв
        laptop.TurnOff();
        tablet.TurnOff();
        smartphone.TurnOff();
    }
}