namespace ConsoleApp2.Frida;

public class FridaProcessMatchOptions
{
    public int? Timeout { get; set; }
    public FridaScope Scope { get; set; } = FridaScope.FRIDA_SCOPE_MINIMAL;
}