/*using System.Diagnostics;

public interface IPayment
{
    void Process(double amount);
    string GetPaymentStatus();
}

public class BkashPayment : IPayment
{
    public void Process(double amount)
    {
        Console.WriteLine("Bkash Payment Process");
        Console.WriteLine(amount + "TK");
    }

    public string GetPaymentStatus()
    {
        return "Bkash Payment Completed";
    }
}
public class NagadPayment : IPayment
{
    public void Process(double amount)
    {
        Console.WriteLine("Nagad Payment Process");
        Console.WriteLine(amount + "TK");
    }

    public string GetPaymentStatus()
    {
        return "Nagad Payment Completed";
    }
}

public class StripeGateway
{
    public void MakePayment(decimal amountInUsd, string currency)
    {
        Console.WriteLine($"Stripe: Charged ${amountInUsd} {currency}");
    }

    public string CheckStatus()
    {
        return "stripe_status: succeeded";
    }
}

public class StripePaymentAdapter : IPayment
{
    private readonly StripeGateway _stripeGateway;
    private const double BDT_TO_USD_RATE = 118;

    public StripePaymentAdapter()
    {
        _stripeGateway = new StripeGateway();
    }
    
    public void Process(double amount)
    {
        decimal amountInUSD = (decimal)(amount/BDT_TO_USD_RATE);
        amountInUSD = Math.Round(amountInUSD, 2);
        _stripeGateway.MakePayment(amountInUSD, "USD");
    }

    public string GetPaymentStatus()
    {
        return _stripeGateway.CheckStatus();
    }
}

public class PaymentFactory
{
    public IPayment CreatePayment(string method)
    {
        return method switch
        {
            "bkash" => new BkashPayment(),
            "nagad" => new NagadPayment(),
            "stripe" => new StripePaymentAdapter(),
                _ => throw new NotImplementedException("Unknown method")
        };
    }
}

public class OrderService
{
    private readonly IPayment _payment;
    public OrderService(IPayment payment)
    {
        _payment = payment;
    }

    public void PlaceOrder(string product, double amount)
    {
        Console.WriteLine($"Order: {product} - {amount} TK");
        _payment.Process(amount);
        Console.WriteLine($"   Status: {_payment.GetPaymentStatus()}");
        Console.WriteLine("Order complete!\n");
    }
}

class Program
{
    public static void Main(string[] args)
    {
        IPayment bkashPayment = new BkashPayment(); 
        OrderService bkash = new OrderService(bkashPayment);
        
        bkash.PlaceOrder("Iphone15", 80000);
        bkashPayment.GetPaymentStatus();

        IPayment stripePayment = new StripePaymentAdapter();
        OrderService stripe = new OrderService(stripePayment);
        
        stripe.PlaceOrder("Macbook M5", 145000);
        stripePayment.GetPaymentStatus();
    }
}*/

using System.ComponentModel.DataAnnotations;

public interface ILogger
{
    void Info(string message);
    void Warn(string message);
    void Error(string message);
}

public class ConsoleLogger : ILogger
{
    public void Info(string message) => Console.WriteLine("Info: "+message);
    public void Warn(string message) => Console.WriteLine("Warning: "+message);
    public void Error(string message) => Console.WriteLine("Error: "+message);
}

//Adaptee
public class OldFileLogger
{
    private string _filePath;
    public OldFileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public void WriteLog(string level, string text, DateTime timestamp)
    {
        Console.WriteLine($"[{timestamp:HH:mm:ss}] [{level}] {text} → {_filePath}");
    }
}
//Adapter
public class OldFileLoggerAdapter : ILogger
{
    private OldFileLogger _oldFileLogger;

    public OldFileLoggerAdapter(string _filePath)
    {
        _oldFileLogger = new OldFileLogger(_filePath);
    }

    public void Info(string message)
    {
        _oldFileLogger.WriteLog("Info", message, DateTime.Now);
    }

    public void Warn(string message)
    {
        _oldFileLogger.WriteLog("Warning", message, DateTime.Now);
    }

    public void Error(string message)
    {
        _oldFileLogger.WriteLog("Error", message, DateTime.Now);
    }
}

class Program
{
    public static void Main(string[] args)
    {
        ILogger consoleLogger = new ConsoleLogger();
        consoleLogger.Info("Order Placed");
        consoleLogger.Error("Payment failed");

        ILogger fileLogger = new OldFileLoggerAdapter("app.log");
        fileLogger.Info("Order Placed");
        fileLogger.Error("Payment failed");
    }
}