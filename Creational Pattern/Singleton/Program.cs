class Singleton
{
    private static Singleton instance;
    Singleton()
    {
        
    }

    public static Singleton GetInstance()
    {
        if(instance == null)
        {
            instance = new Singleton();
        }
        return instance;
    }

}

class Program
{
    static void Main(string[] args)
    {
        var s1 =  Singleton.GetInstance();
        var s2 = Singleton.GetInstance();

        if(s1 == s2)
        {
            Console.WriteLine("Both are same");
        }
        else
        {
            Console.WriteLine("Both are different");
        }
    }
}