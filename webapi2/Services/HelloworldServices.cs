public class HelloWorldService : IHelloWorldService
{
    public string GetHelloWorld()
    {
        return "Hola mundo";
    }
}

public interface IHelloWorldService
{
    string GetHelloWorld();
}