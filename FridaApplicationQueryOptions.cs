namespace PInvoke.FridaCore;

public class FridaApplicationQueryOptions
{
    public string? Identifier { get; set; }
    public FridaScope Scope { get; set; } = FridaScope.FRIDA_SCOPE_MINIMAL;
}