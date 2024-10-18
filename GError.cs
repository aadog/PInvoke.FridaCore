using System.Runtime.InteropServices;

namespace PInvoke.FridaCore;

[StructLayout(layoutKind:LayoutKind.Sequential)]
public struct GError
{
    public UInt32 GQuark;
    public int Code;
    public string Message;
}