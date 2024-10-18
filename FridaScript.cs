using System.Runtime.InteropServices;
using PInvoke.Frida;

namespace ConsoleApp2.Frida;

public class FridaScript(IntPtr handle):IFridaObject
{
    public IntPtr Handle { get; set; } = handle;
    

    public void Load()
    {
        IntPtr ptrError=new IntPtr();
        FridaNative.frida_script_load_sync(Handle,IntPtr.Zero,ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }
    public void UnLoad()
    {
        IntPtr ptrError=new IntPtr();
        FridaNative.frida_script_unload_sync(Handle,IntPtr.Zero,ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }
    public bool IsDestroyed()
    {
        return FridaNative.frida_script_is_destroyed(Handle);
    }

    public void Post(string json,IntPtr data)
    {
        FridaNative.frida_script_post(Handle,json,data);
    }

    public void OnMessage(FridaNative.ScriptOnMessageCallback callback)
    {
        On("message",Marshal.GetFunctionPointerForDelegate(callback));
    }
    public void OnDestroyed(FridaNative.GCallback callback)
    {
        On("destroyed",Marshal.GetFunctionPointerForDelegate(callback));
    }
    public void On(string signal,IntPtr callback)
    {
        FridaNative.g_signal_connect_data(Handle,signal,callback,IntPtr.Zero,IntPtr.Zero,GConnectFlags.G_CONNECT_DEFAULT);
    }

}