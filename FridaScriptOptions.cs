namespace PInvoke.FridaCore;

public class FridaScriptOptions
{
    public string? Name { get; set; }
    public FridaScriptRuntime Runtime { get; set; }= FridaScriptRuntime.FRIDA_SCRIPT_RUNTIME_DEFAULT;
}