namespace PInvoke.FridaCore;

public class FridaSpawnOptions
{
    public string[]? Argv { get; set; }
    public string[]? Envp { get; set; }
    public string[]? Env { get; set; }
    public string? Cwd { get; set; }
    public FridaStdio Stdio { get; set; } = FridaStdio.FRIDA_STDIO_INHERIT;
    public IntPtr? Aux { get; set; }
}