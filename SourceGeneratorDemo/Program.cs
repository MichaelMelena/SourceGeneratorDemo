namespace SourceGeneratorConsole;

public partial class Program
{
    public static void Main()
    {
        HelloFrom("Generated code");
    }
    static partial void HelloFrom(string name);
}
