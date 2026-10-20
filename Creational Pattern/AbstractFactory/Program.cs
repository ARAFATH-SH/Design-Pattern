public interface IPayment
{
    void Process(double amount);
}

public interface IShipping
{
    void Ship(string address);
}

public interface IInvoice
{
    void Generate(string orderId, double amount);
}

public class BkashPayment : IPayment
{
    public void Process(double amount)
    {
        Console.WriteLine("Bkash Payment {0}",amount);
    }
}
public class NagadPayment : IPayment
{
    public void Process(double amount)
    {
        Console.WriteLine("Nagad Payment {0}",amount);
    }
}
public class BanglaInvoice : IInvoice
{
    public void Generate(string orderId, double amount)
    {
        Console.WriteLine($"{orderId} - Bangla Invoice {0}",amount);
    }
}
public class EnglishInvoice : IInvoice
{
    public void Generate(string orderId, double amount)
    {
        Console.WriteLine($"{orderId} - English Invoice {0}",amount);
    }
}
public class DhakaShipping : IShipping{
    public void Ship(string address)
    {
        Console.WriteLine("Dhaka Shipping {0}",address);
    }
}
public class ChittagongShipping : IShipping{
    public void Ship(string address)
    {
        Console.WriteLine("Chittagong Shipping {0}",address);
    }
}

public interface IRegionFactory
{
    IPayment CreatePayment();
    IShipping  CreateShipping();
    IInvoice CreateInvoice();
}

public class DhakaFactory : IRegionFactory
{
    public IPayment CreatePayment()
    {
        return new BkashPayment();
    }
    public IInvoice CreateInvoice()
    {
        return new EnglishInvoice();
    }
    public IShipping CreateShipping()
    {
        return new DhakaShipping();
    }
}
public class ChittagongFactory : IRegionFactory
{
    public IPayment CreatePayment()
    {
        return new NagadPayment();
    }
    public IInvoice CreateInvoice()
    {
        return new BanglaInvoice();
    }
    public IShipping CreateShipping()
    {
        return new ChittagongShipping();
    }
}

public class OrderService
{
    public readonly IRegionFactory _regionFactory;
    
    public OrderService(IRegionFactory regionFactory)
    {
        _regionFactory = regionFactory;
    }

    public void PlaceOrder(string product, double amount, string address)
    {
        Console.WriteLine($"Order: {product} - {amount}");
        
        IPayment payment = _regionFactory.CreatePayment();
        payment.Process(amount);
        
        IShipping shipping = _regionFactory.CreateShipping();
        shipping.Ship(address);
        
        IInvoice invoice = _regionFactory.CreateInvoice();
        invoice.Generate("OrID - 0001", amount);
    }
}

class Program
{
    static void Main(string[] args)
    {
        IRegionFactory factory = new DhakaFactory();
        OrderService dhakaOrderService = new OrderService(factory);
        dhakaOrderService.PlaceOrder("iphone15", 150000, "Savar, Dhaka");
    }
}