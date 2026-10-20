using System;
using System.Collections.Generic;

public interface ILogger
{
    public void Log(string message);
}

public class Logger : ILogger
{
    private Logger()
    {
        Console.WriteLine("Logger created");
    }
    private static Logger _instance = null;
    private static object _lock = new object();

    public static Logger GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
            }
        }
        return _instance;
    }
    public void Log(string message)
    {
        Console.WriteLine("This is from log - {0}",message);
    }
}
public interface ISend
{
    public void Send();
}

public interface ILog
{
    public void Log();
}

public interface ISave
{
    public void Save();
}

public class EmailNotify : ISend, ILog, ISave
{
    public string Email { get; set; }

    public void Send()
    {
        Console.WriteLine("Sending email to", Email);
    }

    public void Log()
    {
        ILogger logger = Logger.GetInstance();
        logger.Log("Logging email to" + Email);
    }

    public void Save()
    {
        Console.WriteLine("Saving db to" + Email);
    }
}

public class SMSNotify : ISend, ILog, ISave
{
    public string Phone { get; set; }

    public void Send()
    {
        Console.WriteLine("Sending sms to" + Phone);
    }

    public void Log()
    {
        ILogger logger = Logger.GetInstance();
        logger.Log("Logging sms to" + Phone);
    }

    public void Save()
    {
        Console.WriteLine("Saving db to" + Phone);
    }
}

public class PushNotify : ISend, ILog
{
    public string Token { get; set; }

    public void Send()
    {
        Console.WriteLine("Sending push to" + Token);
    }

    public void Log()
    {
        ILogger logger = Logger.GetInstance();
        logger.Log("Logging push to" + Token);
    }
}

public class NotifyContext
{
    public ISend send { get; set; }
    public ILog log { get; set; }
    public ISave save { get; set; }
    
    public NotifyContext(ISend send, ILog log, ISave save)
    {
        this.send = send;
        this.log = log;
        this.save = save;
    }

    public void process()
    {
        send.Send();
        log.Log();
        if (save != null)
        {
            save.Save();
        }
    }
}

public interface INotificationContextCreator
{
    public NotifyContext Create();
}

public class EmailNotificationContextCreator : INotificationContextCreator
{
    public NotifyContext Create()
    {
        return new NotifyContext(
            new EmailNotify { Email = "test@test.com" },
            new EmailNotify { Email = "test@test.com"},
            new EmailNotify { Email = "test@test.com"}
        );
    }
}

public class SMSNotificationContextCreator : INotificationContextCreator
{
    public NotifyContext Create()
    {
        return new NotifyContext
        (
            new SMSNotify { Phone = "123456" },
            new SMSNotify { Phone = "123456" },
            new SMSNotify { Phone = "123456" }
        );
    }
}

public class PushNotificationContextCreator : INotificationContextCreator
{
    public NotifyContext Create()
    {
        return new NotifyContext
        (
            new PushNotify { Token = "123456789" },
            new PushNotify { Token = "123456789" },
            null
        );
    }
}

class Program
{
    public static void Main()
    {
        INotificationContextCreator emailNotificationContextCreator =
            new EmailNotificationContextCreator();
        NotifyContext emailContext = emailNotificationContextCreator.Create();

        INotificationContextCreator smsNotificationContextCreator =
            new SMSNotificationContextCreator();
        NotifyContext smsContext = smsNotificationContextCreator.Create();

        INotificationContextCreator pushNotificationContextCreator =
            new PushNotificationContextCreator();
        NotifyContext pushContext = pushNotificationContextCreator.Create();

        emailContext.process();
        smsContext.process();
        pushContext.process();
    }
}
