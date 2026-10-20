public interface ISend
{
    void Send();
}
public interface ISave
{
    void Save();
}
public interface ILog
{
    void Log();
}

public class EmailNotify : ISend, ISave, ILog
{
    public string Email { get; set; }
    public void Send()
    {
        Console.WriteLine("Sending email to " + Email);
    }

    public void Log()
    {
        Console.WriteLine("Logging email to " + Email);
    }

    public void Save()
    {
        Console.WriteLine("Saving db to " + Email);
    }
}
public class SMSNotify : ISend, ISave, ILog
{
    public string Phone { get; set; }
    public void Send()
    {
        Console.WriteLine("Sending sms to" + Phone);
    }

    public void Log()
    {
        Console.WriteLine("Logging sms to" + Phone);
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
        Console.WriteLine("Logging sms to" + Token);
    }

}

public class NotifyContext
{
    public ISend Send { get; set; }
    public ILog Log { get; set; }
    public ISave  Save { get; set; }

    public NotifyContext( ILog log, ISend send, ISave save)
    {
        Send = send;
        Log = log;
        Save = save;
    }

    public void Process()
    {
        Send.Send();
        Log.Log();
        if (Save != null)
        {
            Save.Save();
        }
    }
}

public interface INotificationFactory
{
    public ISend CreateSend();
    public ILog CreateLog();
    public ISave CreateSave();
}

public class EmailNotificationFactory : INotificationFactory
{
    public ISend CreateSend()
    {
        return new EmailNotify();
    }

    public ILog CreateLog()
    {
        return new EmailNotify();
    }

    public ISave CreateSave()
    {
        return new EmailNotify();
    }
}

public class SMSNotificationFactory : INotificationFactory
{
    public ISend CreateSend()
    {
        return new SMSNotify();
    }

    public ILog CreateLog()
    {
        return new SMSNotify();
    }

    public ISave CreateSave()
    {
        return new SMSNotify();
    }
}

public class PushNotificationFactory : INotificationFactory
{
    public ISend CreateSend()
    {
        return new PushNotify();
    }

    public ILog CreateLog()
    {
        return new PushNotify();
    }

    public ISave CreateSave()
    {
        return null;
    }
}

public interface INotificationContextCreator
{
    public NotifyContext Create();
}