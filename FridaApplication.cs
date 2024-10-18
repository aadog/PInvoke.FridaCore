using System.Runtime.InteropServices;
using PInvoke.Frida;

namespace ConsoleApp2.Frida;

public class FridaApplication(IntPtr handle):IFridaObject
{
    public IntPtr Handle { get; set; } = handle;
    
    public string GetIdentifier()
    {
        var l= FridaNative.frida_application_get_identifier(Handle);
        return Marshal.PtrToStringUTF8(l)!;
    }
    public string GetName()
    {
        var l=FridaNative.frida_application_get_name(Handle);
        return Marshal.PtrToStringUTF8(l)!;
    }
    public uint GetPid()
    {
        return FridaNative.frida_application_get_pid(Handle);
    }
    public Dictionary<string,object> GetParameters()
    {
        var l=FridaNative.frida_application_get_parameters(Handle);
        return Tools.GHashTableToDictionary(l);
    }
}