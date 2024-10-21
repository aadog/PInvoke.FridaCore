using System.Runtime.InteropServices;

namespace PInvoke.FridaCore;

public static class Frida
{
    
    public static void GBytesUnRef(IntPtr handle)
    {
        FridaNative.g_bytes_unref(handle);
    }
    public static void GObjectUnRef(IntPtr handle)
    {
        FridaNative.g_object_unref(handle);
    }
    public static IntPtr GObjectRef(IntPtr handle)
    {
        return FridaNative.g_object_ref(handle);
    }

    public static void FridaUnref(IFridaObject fridaObject)
    {
        FridaNative.frida_unref(fridaObject.Handle);
    }

    public static string FridaVersionString()
    {
        var v = FridaNative.frida_version_string();
        return Marshal.PtrToStringUTF8(v)!;
    }
    
    public static void FridaVersion(ref uint major, ref uint minor, ref uint micro,ref uint nano)
    {
        FridaNative.frida_version(ref major,ref minor,ref micro,ref nano);
    }

    public static void FridaInit()
    {
        FridaNative.frida_init();
    }
    public static void FridaDeInit()
    {
        FridaNative.frida_deinit();
    }
    public static void FridaShutdown()
    {
        FridaNative.frida_shutdown();
    }
    public static FridaDeviceManager FridaDeviceManagerNew()
    {
        var l = FridaNative.frida_device_manager_new();
        return new FridaDeviceManager(l);
    }
    public static FridaMainLoop FridaMainLoopNew(bool isRunning)
    {
        var l = FridaNative.g_main_loop_new(IntPtr.Zero, isRunning);
        return new FridaMainLoop(l);
    }
}