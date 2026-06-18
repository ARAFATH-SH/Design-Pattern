public interface IComputerBuilder
{
    void SetCPU();
    void SetGPU();
    void SetRAM();
    void SetStorage();

    Computer GetComputer();
}

public class Computer
{
    public string CPU { get; set; }
    public string GPU { get; set; }
    public string RAM { get; set; }
    public string Storage { get; set; }

    public void showSpecs()
    {
        Console.WriteLine($"CPU: {CPU}");
        Console.WriteLine($"GPU: {GPU}");
        Console.WriteLine($"RAM: {RAM}");
        Console.WriteLine($"Storage: {Storage}");
    }
}

public class GamingComputer : IComputerBuilder
{
    private Computer computer = new Computer();

    public void SetCPU()
    {
        computer.CPU = "Intel Core i9";
    }
    public void SetGPU()
    {
        computer.GPU = "NVIDIA GeForce RTX 3080";
    }
    public void SetRAM()    {
        computer.RAM = "32GB DDR4";
    }
    public void SetStorage()
    {
        computer.Storage = "1TB NVMe SSD";
    }

    public Computer GetComputer()
    {
        return computer;
    }

}
class Builder
{
    public void BuildComputer(IComputerBuilder computerBuilder)
    {
        computerBuilder.SetCPU();
        computerBuilder.SetGPU();
        computerBuilder.SetRAM();
        computerBuilder.SetStorage();
    }
}
class Program
{
    static void Main(string[] args)
    {
        Builder builder = new Builder();
        IComputerBuilder gamingComputerBuilder = new GamingComputer();
        builder.BuildComputer(gamingComputerBuilder);
        Computer gamingComputer = gamingComputerBuilder.GetComputer();
        gamingComputer.showSpecs();
    }
}