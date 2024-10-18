using System.Runtime.InteropServices;

namespace PInvoke.FridaCore;

public class FridaProcess(IntPtr handle):IFridaObject
{
    public IntPtr Handle { get; set; } = handle;
    
    public uint GetPid()
    {
        return FridaNative.frida_process_get_pid(Handle);
    }
    public string GetName()
    {
        var l=FridaNative.frida_process_get_name(Handle);
        return Marshal.PtrToStringUTF8(l)!;
    }
    public Dictionary<string,object> GetParameters()
    {
        var l=FridaNative.frida_process_get_parameters(Handle);
        return Tools.GHashTableToDictionary(l);
    }
}