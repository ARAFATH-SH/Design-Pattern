
/*
    step -

        Private Constructor
        Static Instance
        Public Method to get the instance
        Thread Safety using Lazy<T> for lazy initialization
        Modern Approach using DI Container to uses Singleton lifetime - AddSingleton<>()
*/

/*
    Real World Example - 
            
            Logger
            Confuguration
            Cache
            Connection Pool
*/

/*
    Disadvantages -

        Hidden Dependencies
        Thight Coupling
        SRP Violation
        Hard to test
    
    if you are using DI Container, then it is better to use Singleton lifetime instead of implementing Singleton pattern manually also mitigates 
    the disadvantages of Singleton pattern.
*/

/*
public class Logger
{
    private static readonly Lazy<Logger> instance = new Lazy<Logger>(() => new Logger());

    private int logCount = 0;
    private Logger()
    {
        Console.WriteLine("Logger instance created.");
    }

    public static Logger GetInstance() => instance.Value;

    public void Info(string message)
    {
        logCount++;
        Console.WriteLine($"Info {logCount}: {message}");
    }

    public void Error(string message)
    {
        logCount++;
        Console.WriteLine($"Error {logCount}: {message}");
    }

    public void Warning(string message)
    {
        logCount++;
        Console.WriteLine($"Warning {logCount}: {message}");
    }
    public int GetLogCount() => logCount;
}

public class OrderService
{
    public void PlaceOrder(string product, double price)
    {
        Logger.GetInstance().Info($"Order placed for {product} at ${price}");

        if(price > 10000)
        {
            Logger.GetInstance().Warning($"High value order: {product} at ${price}");
        }
        Logger.GetInstance().Info("Order saved to database.");
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var orderService = new OrderService();
        orderService.PlaceOrder("Laptop", 1500);
        orderService.PlaceOrder("Smartphone", 800);
        orderService.PlaceOrder("Luxury Watch", 12000);

        Console.WriteLine($"Total log entries: {Logger.GetInstance().GetLogCount()}");
        
    }
}
*/

public class AppConfiguration
{
    private static readonly Lazy<AppConfiguration> instance = new Lazy<AppConfiguration>(() => new AppConfiguration());

    public string DatabaseServer { get; private set;}
    public string DatabaseName { get; private set;}
    public int MaxConnections { get; private set;}
    public string SmtpServer { get; private set;}
    public bool EnableLogging { get; private set;}

    private AppConfiguration()
    {
        Console.WriteLine("Loading configuration...");

        DatabaseServer = "localhost:8080";
        DatabaseName = "EcommerceDB";
        MaxConnections = 100;
        SmtpServer = "smtp.example.com";
        EnableLogging = true;

        Console.WriteLine("Configuration loaded.");
    }

    public static AppConfiguration GetInstance() => instance.Value;

}

public class DatabaseService
{
    public void Connect()
    {
        var config = AppConfiguration.GetInstance();
        Console.WriteLine($"Connecting to database at {config.DatabaseServer} with max {config.MaxConnections} connections.");

        if (config.EnableLogging)
        {
            Console.WriteLine("Logging is enabled. Connection details will be logged.");
        }
    }
}

public class EmailService
{
    public void SendEmail(string to, string subject)
    {
        var config = AppConfiguration.GetInstance();
        Console.WriteLine($"Sending email to {to} via {config.SmtpServer} with subject '{subject}'.");

        if (config.EnableLogging)
        {
            Console.WriteLine("Logging is enabled. Email details will be logged.");
        }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var dbService = new DatabaseService();
        var emailService = new EmailService();

        dbService.Connect();
        emailService.SendEmail("user@example.com", "Test Email");
    }
}