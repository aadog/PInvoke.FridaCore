using System.Runtime.InteropServices;

namespace ConsoleApp2.Frida;

[StructLayout(layoutKind:LayoutKind.Sequential)]
public struct GError
{
    public UInt32 GQuark;
    public int Code;
    public string Message;
}