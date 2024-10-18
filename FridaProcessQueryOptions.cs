namespace PInvoke.FridaCore;

public class FridaProcessQueryOptions
{
    public uint? SelectPid { get; set; }
    public FridaScope Scope { get; set; } = FridaScope.FRIDA_SCOPE_MINIMAL;
}