namespace PInvoke.FridaCore;

public class FridaRemoteDeviceOptions
{
    public string? Origin { get; set; }
    public string? Token { get; set; }
    public int? KeepaliveInterval { get; set; }
    public IntPtr? GTlsCertificate { get; set; }
}