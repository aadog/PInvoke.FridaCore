using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PInvoke.FridaCore;

public class FridaSession(IntPtr handle) : IFridaObject
{
    public IntPtr Handle { get; set; } = handle;

    public uint GetPid()
    {
        return FridaNative.frida_session_get_pid(Handle);
    }

    public uint GetPersistTimeout()
    {
        return FridaNative.frida_session_get_persist_timeout(Handle);
    }

    public FridaDevice GetDevice()
    {
        var l = FridaNative.frida_session_get_device(Handle);
        return new FridaDevice(l);
    }

    public byte[] CompileScript(string source, FridaScriptOptions options)
    {
        var scriptOptions = FridaNative.frida_script_options_new();
        FridaNative.frida_script_options_set_runtime(scriptOptions, options.Runtime);
        if (options.Name != null)
        {
            FridaNative.frida_script_options_set_name(scriptOptions, options.Name);
        }

        try
        {
            IntPtr ptrError = new IntPtr();
            var p = FridaNative.frida_session_compile_script_sync(Handle, source, scriptOptions, IntPtr.Zero,
                ref ptrError);
            if (ptrError != IntPtr.Zero)
            {
                var err = Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }

            int n = 0;
            var b = FridaNative.g_bytes_get_data(p, ref n);
            FridaNative.g_bytes_unref(p);
            var result = new byte[n];
            Marshal.Copy(b, result, 0, n);
            return result;
        }
        finally
        {
            FridaNative.g_object_unref(scriptOptions);
        }
    }

    public FridaScript CreateScript(byte[] source, FridaScriptOptions options)
    {
        var scriptOptions = FridaNative.frida_script_options_new();
        FridaNative.frida_script_options_set_runtime(scriptOptions, options.Runtime);
        if (options.Name != null)
        {
            FridaNative.frida_script_options_set_name(scriptOptions, options.Name);
        }

        try
        {
            var bufSource=new byte[source.Length+1];
            source.CopyTo(bufSource, 0);
            IntPtr ptrError = new IntPtr();
            var p = FridaNative.frida_session_create_script_sync(Handle, bufSource, scriptOptions, IntPtr.Zero,
                ref ptrError);
            if (ptrError != IntPtr.Zero)
            {
                var err = Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }

            return new FridaScript(p);
        }
        finally
        {
            FridaNative.g_object_unref(scriptOptions);
        }
    }

    public void Detach()
    {
        IntPtr ptrError = new IntPtr();
        var p = FridaNative.frida_session_detach_sync(Handle, IntPtr.Zero, ref ptrError);
        if (ptrError != IntPtr.Zero)
        {
            var err = Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }


    public void OnDetached(FridaNative.SessionOnDetached callback)
    {
        On("detached", Marshal.GetFunctionPointerForDelegate(callback));
    }

    public void On(string signal, IntPtr callback)
    {
        FridaNative.g_signal_connect_data(Handle, signal, callback, IntPtr.Zero, IntPtr.Zero,
            GConnectFlags.G_CONNECT_DEFAULT);
    }
}