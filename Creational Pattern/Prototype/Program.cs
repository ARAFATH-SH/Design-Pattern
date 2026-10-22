public interface IPrototype<T>
{
    T Clone();
}

/*public class OrderItem
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }

    public OrderItem(string productName, int quantity, double unitPrice)
    {
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public OrderItem Clone()
    {
        return new OrderItem(ProductName, Quantity, UnitPrice);
    }
}

public class CorporateOrder : IPrototype<CorporateOrder>
{
    public string OrderId { get; set; }
    public string ClientName { get; set; }
    public string ContactPerson { get; set; }
    public string ShippingAddress { get; set; }
    public string BillingAddress { get; set; }
    public string PaymentTerms { get; set; }
    public double DiscountPercentage { get; set; }
    public string Priority { get; set; }
    public string SpecialInstructions { get; set; }
    public string DeliveryTime { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public DateTime OrderDate { get; set; }

    public CorporateOrder Clone()
    {
        var clone = new CorporateOrder();

        clone.OrderId = OrderId;
        clone.ClientName = ClientName;
        clone.ContactPerson = ContactPerson;
        clone.ShippingAddress = ShippingAddress;
        clone.BillingAddress = BillingAddress;
        clone.PaymentTerms = PaymentTerms;
        clone.DiscountPercentage = DiscountPercentage;
        clone.Priority = Priority;
        clone.SpecialInstructions = SpecialInstructions;
        clone.DeliveryTime = DeliveryTime;
        clone.OrderDate = OrderDate;

        clone.Items = new List<OrderItem>();

        foreach (var item in Items)
        {
            clone.Items.Add(item.Clone());
        }
        return clone;
    }

    public double GetTotal()
    {
        double subTotal = 0;
        foreach (var item in Items)
        {
            subTotal += item.Quantity * item.UnitPrice;
        }

        return subTotal * (1 - DiscountPercentage/100);
    }

    public void Print()
    {
        Console.WriteLine($"Order: {OrderId} | Date: {OrderDate:yyyy-MM-dd}");
        Console.WriteLine($"   Client: {ClientName} ({ContactPerson})");
        Console.WriteLine($"   Ship to: {ShippingAddress}");
        Console.WriteLine($"   Payment: {PaymentTerms} | Discount: {DiscountPercentage}%");
        Console.WriteLine($"   Priority: {Priority}");
        Console.WriteLine($"   Special: {SpecialInstructions}");
        Console.WriteLine($"   Items ({Items.Count}):");
        foreach (var item in Items)
            Console.WriteLine($"     - {item.ProductName} x{item.Quantity} @ {item.UnitPrice:N0} TK");
        Console.WriteLine($"   💰 Total: {GetTotal():N0} TK");
    }
}

public class Program
{
    static List<OrderItem> LoadItemsFromDatabase()
    {
        Console.WriteLine("Loading items from database...");

        return new List<OrderItem>
        {
            new OrderItem("Laptop Dell XPS 15", 20, 150000),
            new OrderItem("Monitor LG 27\"", 20, 35000),
            new OrderItem("Keyboard Mechanical", 50, 5000)
        };
    }

    static void Main(string[] args)
    {
        var template = new CorporateOrder();
        template.ClientName = "TechCorp Ltd";
        template.ContactPerson = "Mr. Karim";
        template.ShippingAddress = "Gulshan-2, Dhaka";
        template.BillingAddress = "Motijheel, Dhaka";
        template.PaymentTerms = "Net 30";
        template.DiscountPercentage = 15;
        template.Priority = "High";
        template.SpecialInstructions = "Deliver before 10 AM, call before delivery";
        template.DeliveryTime = "Within 3 business days";
        template.Items = LoadItemsFromDatabase();
        Console.WriteLine("Template ready!\n");
        
        Console.WriteLine("January Order");
        var janOrder = template.Clone();
        janOrder.OrderId = "ORD-JAN-001";
        janOrder.OrderDate = new DateTime(2000, 1, 1);
        janOrder.Print();
        
        Console.WriteLine("\nFebruary Order\n");
        var frbOrder = template.Clone();
        frbOrder.OrderId = "ORD-FEB-001";
        frbOrder.OrderDate = new DateTime(2000, 2, 1);
        frbOrder.Items[1].Quantity = 30;
        frbOrder.Items.Add(new OrderItem("Macbook", 20, 150000));
        frbOrder.Print();
        
        Console.WriteLine("\n=== Template Order (Original) ===");
        Console.WriteLine($"Template items: {template.Items.Count}");       
        Console.WriteLine($"Template Monitor qty: {template.Items[1].Quantity}");  
       
    }
}*/

//Environment Configuration

public class AppConfiguration : IPrototype<AppConfiguration>
{
    public string AppName { get; set; }
    public string Version { get; set; }
    public string DatabaseServer { get; set; }
    public string DatabaseName { get; set; }
    public int MaxConnections { get; set; }
    public string LogLevel { get; set; }
    public bool EnableCache { get; set; }
    public string SmtpServer {  get; set; }
    public int SessionTimeout { get; set; }
    public List<string> AllowedOrigins { get; set; }

    public AppConfiguration Clone()
    {
        var clone = new AppConfiguration();
        clone.AppName =  AppName;
        clone.Version = Version;
        clone.DatabaseServer = DatabaseServer;
        clone.DatabaseName = DatabaseName;
        clone.MaxConnections = MaxConnections;
        clone.LogLevel = LogLevel;
        clone.EnableCache = EnableCache;
        clone.SmtpServer = SmtpServer;
        clone.SessionTimeout = SessionTimeout;

        clone.AllowedOrigins = new List<string>(AllowedOrigins);
        return clone;
    }
    public void Print(string env)
    {
        Console.WriteLine($"[{env}] {AppName} v{Version}");
        Console.WriteLine($"   DB: {DatabaseServer}/{DatabaseName} (max: {MaxConnections})");
        Console.WriteLine($"   Log: {LogLevel} | Cache: {EnableCache} | Session: {SessionTimeout}min");
        Console.WriteLine($"   SMTP: {SmtpServer}");
        Console.WriteLine($"   Origins: {string.Join(", ", AllowedOrigins)}\n");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var devConfig = new AppConfiguration();
        devConfig.AppName = "YOYO";
        devConfig.Version = "1.0";
        devConfig.DatabaseServer = "localhost";
        devConfig.DatabaseName = "dev";
        devConfig.MaxConnections = 100;
        devConfig.LogLevel = "Debug";
        devConfig.EnableCache = true;
        devConfig.SmtpServer = "127.0.0.1";
        devConfig.SessionTimeout = 5000;
        devConfig.AllowedOrigins = new List<string>{"http://localhost:8080"};
        devConfig.Print("Development");
        
        //Production
        var production = devConfig.Clone();
        production.AppName = "YOYO";
        production.Version = "1.0";
        production.DatabaseServer = "prod-db.yoyo.io";
        production.DatabaseName = "yoyo-prod";
        production.MaxConnections = 100;
        production.LogLevel = "Error";
        production.EnableCache = true;
        production.SmtpServer = "smtp.yoyo.io";
        production.SessionTimeout = 30;
        production.AllowedOrigins = new List<string>
        {
            "https://yoyo.io",
            "https://www.yoyo.io"
        };
        production.Print("Production");

    }
}