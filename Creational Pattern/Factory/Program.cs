public interface IPayment
{
    void process(double amount);
}

public class BkashPayment : IPayment
{
    public void process(double amount)
    {
        Console.WriteLine("Bkash Payment");
        Console.WriteLine(amount);
    }
}

public class NagadPayment : IPayment
{
    public void process(double amount)
    {
        Console.WriteLine("Nagad Payment");
        Console.WriteLine(amount);
    }
}

public class RocketPayment : IPayment
{
    public void process(double amount)
    {
        Console.WriteLine("Rocket Payment");
        Console.WriteLine(amount);
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
            "Rocket" => new RocketPayment(),
            _ => throw new ArgumentException($"Unknown payment method: {method}")
        };
    }
}

public class OrderService
{
    private readonly PaymentFactory _paymentFactory;

    public OrderService(PaymentFactory paymentFactory)
    {
        _paymentFactory = paymentFactory;
    }

    public void PlaceOrder(string product, double amount, string method)
    {
        Console.WriteLine($"Order Placed: {product} - {amount} Tk");

        IPayment payment = _paymentFactory.CreatePayment(method);
        payment.process(amount);
        Console.WriteLine("Order Completed");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        PaymentFactory factory = new PaymentFactory();
        OrderService orderService = new OrderService(factory);
        
        orderService.PlaceOrder("Iphone15", 150000, "bkash");
    }
}