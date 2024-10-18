namespace PInvoke.FridaCore;

public class FridaSessionOptions
{
    public FridaRealm Realm { get; set; } =  FridaRealm.FRIDA_REALM_NATIVE;
    public uint? PersistTimeout { get; set; }
    public string? AgentPath{ get; set; }
}