using System.Runtime.InteropServices;

namespace PInvoke.FridaCore;

public class FridaDeviceManager(IntPtr handle):IFridaObject
{
    public IntPtr Handle { get; set; } = handle;
    
    public List<FridaDevice> EnumerateDevices()
    {
        IntPtr ptrError=new IntPtr();
        var devices = new List<FridaDevice>();
        var deviceList=FridaNative.frida_device_manager_enumerate_devices_sync(Handle, IntPtr.Zero, ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
        var size = FridaNative.frida_device_list_size(deviceList);
        for (int i = 0; i < size; i++)
        {
            var h = FridaNative.frida_device_list_get(deviceList, i);
            devices.Add(new FridaDevice(h));
        }
        FridaNative.g_object_unref(deviceList);
        return devices;
    }

    public FridaDevice AddRemoteDevice(string address,FridaRemoteDeviceOptions options)
    {
        var remoteDeviceOptions = FridaNative.frida_remote_device_options_new();
        if (options.Origin != null)
        {
            FridaNative.frida_remote_device_options_set_origin(remoteDeviceOptions,options.Origin);
        }
        if (options.Token != null)
        {
            FridaNative.frida_remote_device_options_set_token(remoteDeviceOptions,options.Token);
        }
        if (options.KeepaliveInterval != null)
        {
            FridaNative.frida_remote_device_options_set_keepalive_interval(remoteDeviceOptions,options.KeepaliveInterval.Value);
        }
        try
        {
            IntPtr ptrError=new IntPtr();
            var h=FridaNative.frida_device_manager_add_remote_device_sync(
                Handle,
                address,
                remoteDeviceOptions, 
                IntPtr.Zero, 
                ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }
            return new FridaDevice(h);
        }
        finally
        {
            FridaNative.g_object_unref(remoteDeviceOptions);
        }
    }

    public void RemoveRemoteDevice(string address)
    {
        IntPtr ptrError=new IntPtr();
        FridaNative.frida_device_manager_remove_remote_device_sync(
            Handle,
            address,
            IntPtr.Zero,
            ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }
    public void Close()
    {
        IntPtr ptrError=new IntPtr();
        FridaNative.frida_device_manager_close_sync(
            Handle,
            IntPtr.Zero,
            ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }

    public FridaDevice? FindDeviceByType(FridaDeviceType deviceType,int timeout)
    {
        IntPtr ptrError=new IntPtr();
        var h=FridaNative.frida_device_manager_find_device_by_type_sync(
            Handle,
            deviceType,
            timeout, 
            IntPtr.Zero, 
            ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
        if (h == IntPtr.Zero)
        {
            return null;
        }
        return new FridaDevice(h);
    }
    public FridaDevice? FindDeviceById(string id,int timeout)
    {
        IntPtr ptrError=new IntPtr();
        var h=FridaNative.frida_device_manager_find_device_by_id_sync(
            Handle,
            id,
            timeout, 
            IntPtr.Zero, 
            ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
        if (h == IntPtr.Zero)
        {
            return null;
        }
        return new FridaDevice(h);
    }
    
    
    public void OnChange(FridaNative.GCallback callback)
    {
        On("changed", callback);
    }   
    public void OnRemoved(IntPtr device,FridaNative.GCallback callback)
    {
        On("changed", callback);
    }   
    public void OnAdded(IntPtr device,FridaNative.GCallback callback)
    {
        On("added", callback);
    }   
    public void On(string signal,Delegate callback)
    {
        FridaNative.g_signal_connect_data(Handle,signal,callback,IntPtr.Zero,IntPtr.Zero,GConnectFlags.G_CONNECT_DEFAULT);
    }

}



