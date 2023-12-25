partial class Program
{
    private static void Main(string[] args)
    {
        Welcome6230();
        Welcome1640();
        Console.ReadKey();
    }

    private static void Welcome6230()
    {
        Console.WriteLine("Enter your nuame: ");
        String name = Console.ReadLine();
        Console.WriteLine("{0}, welcome to my first program", name);
    }

     static partial void Welcome1640();
}