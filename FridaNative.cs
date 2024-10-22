using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;

namespace PInvoke.FridaCore;

public enum FridaDeviceType
{
    [Display(Name = "Local")]
    FridaDeviceTypeLocal,
    [Display(Name = "Remote")]
    FridaDeviceTypeRemote,
    [Display(Name = "Usb")]
    FridaDeviceTypeUsb
};

public enum GConnectFlags
{
    G_CONNECT_DEFAULT = 0,
    G_CONNECT_AFTER = 1 << 0,
    G_CONNECT_SWAPPED = 1 << 1
}

public enum FridaScriptRuntime
{
    FRIDA_SCRIPT_RUNTIME_DEFAULT,
    FRIDA_SCRIPT_RUNTIME_QJS,
    FRIDA_SCRIPT_RUNTIME_V8
};

public enum FridaScope
{
    FRIDA_SCOPE_MINIMAL,
    FRIDA_SCOPE_METADATA,
    FRIDA_SCOPE_FULL
};

public enum FridaRealm
{
    FRIDA_REALM_NATIVE,
    FRIDA_REALM_EMULATED
};

public enum FridaStdio
{
    FRIDA_STDIO_INHERIT,
    FRIDA_STDIO_PIPE
};
public class FridaNative
{
    const string DllName = "FridaCoreSharedWindows";
    const CallingConvention DllCallingConvention = CallingConvention.Cdecl;
    public static IntPtr GVariantStringType = g_variant_type_new("s");
    public static IntPtr GVariantInt64Type = g_variant_type_new("x");
    public static IntPtr GVariantBooleanType = g_variant_type_new("b");
    public static IntPtr GVariantVariantType = g_variant_type_new("v");
    public static IntPtr GVariantByteArrayType = g_variant_type_new("ay");
    public static IntPtr GVariantVarDictType = g_variant_type_new("{sv}");
    public static IntPtr GVariantArrayType = g_variant_type_new("a*");


    public delegate void GHFunc(IntPtr key, IntPtr value, IntPtr userData);

    public delegate void GCallback();

    public delegate void ScriptOnMessageCallback(IntPtr script, [MarshalAs(UnmanagedType.LPUTF8Str)]string message, IntPtr data, IntPtr userData);

    public delegate void SessionOnDetached(IntPtr session, IntPtr reason, IntPtr crash, IntPtr userData);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_version_string();
    
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_version(ref uint major, ref uint minor, ref uint micro,ref uint nano);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_init();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_deinit();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_shutdown();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_manager_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_manager_enumerate_devices_sync(IntPtr manager, IntPtr cancellable,
        ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_manager_add_remote_device_sync(IntPtr manager, [MarshalAs(UnmanagedType.LPUTF8Str)] string address,
        IntPtr options, IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_manager_remove_remote_device_sync(IntPtr manager, [MarshalAs(UnmanagedType.LPUTF8Str)] string address,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_manager_find_device_by_type_sync(IntPtr manager, FridaDeviceType type,
        int timeout, IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_manager_find_device_by_id_sync(IntPtr manager, [MarshalAs(UnmanagedType.LPUTF8Str)] string id, int timeout,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_manager_close_sync(IntPtr manager, IntPtr cancellable, ref IntPtr error);

    /* DeviceList */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern int frida_device_list_size(IntPtr deviceList);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_list_get(IntPtr deviceList, int index);

    /* Device */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_id(IntPtr device);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_name(IntPtr device);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_icon(IntPtr device);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern FridaDeviceType frida_device_get_dtype(IntPtr device);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_bus(IntPtr device);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_manager(IntPtr device);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool frida_device_is_lost(IntPtr device);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_query_system_parameters_sync(IntPtr device, IntPtr cancellable,
        ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_enumerate_processes_sync(IntPtr device, IntPtr options, IntPtr cancellable,
        ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_enumerate_applications_sync(IntPtr device, IntPtr options,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_find_process_by_name_sync(IntPtr device, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, IntPtr options,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_find_process_by_pid_sync(IntPtr device, uint pid, IntPtr options,
        IntPtr cancellable, ref IntPtr error);
    
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_process_by_name_sync(IntPtr device, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, IntPtr options,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_process_by_pid_sync(IntPtr device, uint pid, IntPtr options,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_get_frontmost_application_sync(IntPtr device, IntPtr options,
        IntPtr cancellable, ref IntPtr error);


    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern uint frida_device_spawn_sync(IntPtr device,[MarshalAs(UnmanagedType.LPUTF8Str)]string program, IntPtr options, IntPtr cancellable,
        ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_device_attach_sync(IntPtr device, uint pid, IntPtr options, IntPtr cancellable,
        ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_device_kill_sync(IntPtr device, uint pid, IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_device_resume_sync(IntPtr device, uint pid, IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_device_input_sync(IntPtr device, uint pid, IntPtr data, IntPtr cancellable,
        IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_device_inject_library_file_sync(IntPtr device, uint pid, IntPtr path,
        IntPtr entrypoint, IntPtr data, IntPtr cancellable, IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_device_inject_library_blob_sync(IntPtr device, uint pid, IntPtr blob,
        IntPtr entrypoint, IntPtr data, IntPtr cancellable, IntPtr error);


    /* ApplicationList */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern int frida_application_list_size(IntPtr applicationList);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_application_list_get(IntPtr processList, int index);

    /* ProcessList */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern int frida_process_list_size(IntPtr processList);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_process_list_get(IntPtr processList, int index);


    /* __G_hash_table__ */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern int g_hash_table_size(IntPtr hashTable);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_hash_table_get_keys(IntPtr hashTable);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_hash_table_get_values(IntPtr hashTable);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_hash_table_lookup(IntPtr hashTable, IntPtr key);


    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_hash_table_foreach(IntPtr hashTable,
        [MarshalAs(UnmanagedType.FunctionPtr)] GHFunc ghFunc, IntPtr userData);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_hash_table_iter_init(IntPtr iter, IntPtr hashTable);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_hash_table_iter_remove(IntPtr iter);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool g_hash_table_iter_next(IntPtr iter, ref IntPtr key, ref IntPtr value);


    /*g_list*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern int g_list_length(IntPtr list);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_list_nth_data(IntPtr list, int n);

    /*GVariant Type*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_type_new([MarshalAs(UnmanagedType.LPUTF8Str)] string type_string);

    /*GVariant*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_get_type_string(IntPtr variant);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool g_variant_is_of_type(IntPtr variant, IntPtr type);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_get_string(IntPtr variant, IntPtr size);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern Int64 g_variant_get_int64(IntPtr variant);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool g_variant_get_boolean(IntPtr variant);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_get_variant(IntPtr variant);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_get_fixed_array(IntPtr variant, ref int nElements, int elementSize);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_iter_new(IntPtr variant);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern Int64 g_variant_iter_init(IntPtr iter, IntPtr variant);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_variant_iter_free(IntPtr variant);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_iter_next_value(IntPtr iter);
    
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool g_variant_iter_loop(IntPtr iter,[MarshalAs(UnmanagedType.LPUTF8Str)] string format_string,out IntPtr key,out IntPtr value);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_variant_get_child_value(IntPtr iter, int index);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_variant_unref(IntPtr variant);
    // [DllImport(DllName)]
    // public static extern IntPtr g_variant_get_type_string(IntPtr list);

    /* Process */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern uint frida_process_get_pid(IntPtr process);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_process_get_name(IntPtr process);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_process_get_parameters(IntPtr process);

    /* Application */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_application_get_identifier(IntPtr application);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_application_get_name(IntPtr application);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern uint frida_application_get_pid(IntPtr application);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_application_get_parameters(IntPtr application);


    /* Session */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern uint frida_session_get_pid(IntPtr session);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern uint frida_session_get_persist_timeout(IntPtr session);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_get_device(IntPtr session);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_compile_script_sync(IntPtr session,[MarshalAs(UnmanagedType.LPUTF8Str)]string source, IntPtr options,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_create_script_sync(IntPtr session,[MarshalAs(UnmanagedType.LPUTF8Str)]string source, IntPtr options,
        IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_snapshot_script_sync(IntPtr session,[MarshalAs(UnmanagedType.LPUTF8Str)] string embed_script, IntPtr options,
        IntPtr cancellable, IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_create_script_from_bytes_sync(IntPtr session, IntPtr bytes,
        IntPtr options, IntPtr cancellable, IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_detach_sync(IntPtr session, IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_resume_sync(IntPtr session, IntPtr cancellable, IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool frida_session_is_detached(IntPtr session);


    /*G_BYTES*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_bytes_ref(IntPtr bytes);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_bytes_unref(IntPtr bytes);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_bytes_unref_to_data(IntPtr bytes, ref int size);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern int g_bytes_get_size(IntPtr bytes);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_bytes_get_data(IntPtr bytes, ref int size);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_bytes_new_static(IntPtr data, int size);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_bytes_new([MarshalAs(UnmanagedType.LPUTF8Str)] string data, int size);


    /* Script */
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool frida_script_is_destroyed(IntPtr script);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_load_sync(IntPtr script, IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_unload_sync(IntPtr script, IntPtr cancellable, ref IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_eternalize_sync(IntPtr script, IntPtr cancellable, IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_post(IntPtr script,[MarshalAs(UnmanagedType.LPUTF8Str)] string json, IntPtr data);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_enable_debugger_sync(IntPtr script, Int16 port, IntPtr cancellable,
        IntPtr error);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_disable_debugger_sync(IntPtr script, IntPtr cancellable, IntPtr error);

    /*Script Options*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_script_options_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_options_set_runtime(IntPtr scriptOptions, FridaScriptRuntime runtime);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_script_options_set_name(IntPtr scriptOptions, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);


    /*frontmost_query options*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_frontmost_query_options_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_frontmost_query_options_set_scope(IntPtr options, FridaScope scope);

    /*application query options*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_application_query_options_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_application_query_options_select_identifier(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string identifier);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_application_query_options_set_scope(IntPtr options, FridaScope scope);


    /*session options*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_session_options_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_session_options_set_realm(IntPtr options, FridaRealm ream);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_session_options_set_persist_timeout(IntPtr options, uint persistTimeout);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_session_options_set_emulated_agent_path(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string agentPath);


    /*process match options*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_process_match_options_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_process_match_options_set_timeout(IntPtr options, int timeout);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_process_match_options_set_scope(IntPtr options, FridaScope scope);

    /*process query options*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_process_query_options_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_process_query_options_select_pid(IntPtr options, uint pid);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_process_query_options_set_scope(IntPtr options, FridaScope scope);

    /*frida_spawn_options_new*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_spawn_options_new();
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_spawn_options_set_argv(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string[] argv,int size);
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_spawn_options_set_envp(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string[] envp,int size);
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_spawn_options_set_env(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string[] env,int size);
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_spawn_options_set_cwd(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string cwd);
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_spawn_options_set_stdio(IntPtr options, FridaStdio stdio);
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_spawn_options_set_aux(IntPtr options, IntPtr aux);
    
    

    /*frida_remote_device_options*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr frida_remote_device_options_new();

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_remote_device_options_set_certificate(IntPtr options, IntPtr certificate);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_remote_device_options_set_origin(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string origin);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_remote_device_options_set_token(IntPtr options, [MarshalAs(UnmanagedType.LPUTF8Str)] string token);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_remote_device_options_set_keepalive_interval(IntPtr options, int keepaliveInterval);


    /*g_object*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_object_get_property(IntPtr obj, [MarshalAs(UnmanagedType.LPUTF8Str)] string property);
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_object_ref(IntPtr obj);
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_object_unref(IntPtr obj);

    /*g_value*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_value_get_string(IntPtr obj);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_value_get_pointer(IntPtr obj);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern double g_value_get_double(IntPtr obj);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern float g_value_get_float(IntPtr obj);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern int g_value_get_int(IntPtr obj);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern Boolean g_value_get_boolean(IntPtr obj);

    /*g_error*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_error_free(IntPtr obj);


    /*g_signal*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern UInt64 g_signal_connect_data(IntPtr instance, [MarshalAs(UnmanagedType.LPUTF8Str)] string detailed_signal, Delegate c_handler,
        IntPtr data, IntPtr destroy_data, GConnectFlags connect_flags);
    

    /*g_main*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern IntPtr g_main_loop_new(IntPtr context, bool is_running);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern bool g_main_loop_is_running(IntPtr loop);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_main_loop_run(IntPtr loop);

    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_main_loop_unref(IntPtr loop);


    /*frida*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void frida_unref(IntPtr instance);


    /*g_clear*/
    [DllImport(DllName, CallingConvention=DllCallingConvention)]
    public static extern void g_clear_object(IntPtr obj);
}