namespace CsharpTBCAcademy.Assignment16;

public class MyCustomException: Exception
{
    public MyCustomException(string message) : base(message)
    {
        Console.WriteLine(message);
    }
}