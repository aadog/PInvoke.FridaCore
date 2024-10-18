using System.Runtime.InteropServices;
using PInvoke.Frida;

namespace ConsoleApp2.Frida;

public class FridaDevice(IntPtr handle):IFridaObject
{
    public IntPtr Handle { get; set; } = handle;
    
    public FridaDeviceManager GetDeviceManager()
    {
        var l = FridaNative.frida_device_get_manager(Handle);
        return new FridaDeviceManager(l);
    }

    public string GetName()
    {
        var h=FridaNative.frida_device_get_name(Handle);
        return Marshal.PtrToStringAnsi(h)??"";
    }
    public string GetId()
    {
        var h=FridaNative.frida_device_get_id(Handle);
        return Marshal.PtrToStringAnsi(h)??"";
    }

    public FridaDeviceType GetDType() => FridaNative.frida_device_get_dtype(Handle);
    
    public Dictionary<string,object> QuerySystemParameters()
    {
        IntPtr ptrError=new IntPtr();
        var h=FridaNative.frida_device_query_system_parameters_sync(Handle,IntPtr.Zero, ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
        var d = Tools.GHashTableToDictionary(h);
        return d;
    }
    
    public List<FridaProcess> EnumerateProcessList(FridaProcessQueryOptions options)
    {
        var queryOptions = FridaNative.frida_process_query_options_new();
        FridaNative.frida_process_query_options_set_scope(queryOptions, options.Scope);
        if (options.SelectPid != null)
        {
            FridaNative.frida_process_query_options_select_pid(queryOptions,options.SelectPid.Value);
        }
        try
        {
            IntPtr ptrError=new IntPtr();
            var result = new List<FridaProcess>();
            var l=FridaNative.frida_device_enumerate_processes_sync(Handle,queryOptions, IntPtr.Zero,ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }
            var n = FridaNative.frida_process_list_size(l);
            for (var i = 0; i < n; i++)
            {
                var p = FridaNative.frida_process_list_get(l,i);
                result.Add(new FridaProcess(p));
            }
            return result;
        }
        finally
        {
            FridaNative.g_object_unref(queryOptions);
        }
        
    }
    
    public List<FridaApplication> EnumerateApplicationList(FridaApplicationQueryOptions options)
    {
        var queryOptions = FridaNative.frida_application_query_options_new();
        FridaNative.frida_application_query_options_set_scope(queryOptions, options.Scope);
        if (options.Identifier != null)
        {
            FridaNative.frida_application_query_options_select_identifier(queryOptions,options.Identifier);
        }

        try
        {
            IntPtr ptrError=new IntPtr();
            var result = new List<FridaApplication>();
            var l=FridaNative.frida_device_enumerate_applications_sync(Handle,queryOptions, IntPtr.Zero, ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }
            var n = FridaNative.frida_application_list_size(l);
            for (var i = 0; i < n; i++)
            {
                var p = FridaNative.frida_application_list_get(l,i);
                result.Add(new FridaApplication(p));
            }
            return result;
        }
        finally
        {
            FridaNative.g_object_unref(queryOptions);
        }
    }

    public FridaApplication FrontMostApplication(FridaFrontMostQueryOptions options)
    {
        var queryOptions = FridaNative.frida_frontmost_query_options_new();
        FridaNative.frida_frontmost_query_options_set_scope(queryOptions, options.Scope);
        try
        {
            IntPtr ptrError=new IntPtr();
            var l=FridaNative.frida_device_get_frontmost_application_sync(Handle,queryOptions, IntPtr.Zero, ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }
            return new FridaApplication(l);
        }
        finally
        {
            FridaNative.g_object_unref(queryOptions);
        }
        
    }

    public FridaProcess? FindProcessByName(string name,FridaProcessMatchOptions options)
    {
        var queryOptions = FridaNative.frida_process_match_options_new();
        FridaNative.frida_process_match_options_set_scope(queryOptions, options.Scope);
        if (options.Timeout != null)
        {
            FridaNative.frida_process_match_options_set_timeout(queryOptions,options.Timeout.Value);
        }
        try
        {
            IntPtr ptrError=new IntPtr();
            var l=FridaNative.frida_device_find_process_by_name_sync(Handle,name, queryOptions, IntPtr.Zero,ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }

            if (l == IntPtr.Zero)
            {
                return null;
            }

            return new FridaProcess(l);
        }
        finally
        {
            FridaNative.g_object_unref(queryOptions);
        }
    }
    
    public FridaProcess? FindProcessByPid(int pid,FridaProcessMatchOptions options)
    {
        var queryOptions = FridaNative.frida_process_match_options_new();
        FridaNative.frida_process_match_options_set_scope(queryOptions, options.Scope);
        if (options.Timeout != null)
        {
            FridaNative.frida_process_match_options_set_timeout(queryOptions,options.Timeout.Value);
        }

        try
        {
            IntPtr ptrError=new IntPtr();
            var l=FridaNative.frida_device_find_process_by_pid_sync(Handle,pid, queryOptions, IntPtr.Zero,ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }
            if (l == IntPtr.Zero)
            {
                return null;
            }
            return new FridaProcess(l);
        }
        finally
        {
            FridaNative.g_object_unref(queryOptions);
        }
    }

    public void KillProcess(uint pid)
    {
        IntPtr ptrError=new IntPtr();
        FridaNative.frida_device_kill_sync(Handle,pid,IntPtr.Zero, ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }
    public FridaSession AttachProcess(uint pid,FridaSessionOptions options)
    {
        var sessionOptions = FridaNative.frida_session_options_new();
        FridaNative.frida_session_options_set_realm(sessionOptions, options.Realm);
        if (options.PersistTimeout != null)
        {
            FridaNative.frida_session_options_set_persist_timeout(sessionOptions, options.PersistTimeout.Value);
        }
        if (options.AgentPath != null)
        {
            FridaNative.frida_session_options_set_emulated_agent_path(sessionOptions, options.AgentPath);
        }

        try
        {
            IntPtr ptrError=new IntPtr();
            var l=FridaNative.frida_device_attach_sync(Handle,pid,sessionOptions, IntPtr.Zero,ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }
            return new FridaSession(l);
        }
        finally
        {
            FridaNative.g_object_unref(sessionOptions);
        }
    }
    public void ResumeProcess(uint pid)
    {
        IntPtr ptrError=new IntPtr();
        FridaNative.frida_device_resume_sync(Handle,pid,IntPtr.Zero, ref ptrError);
        if(ptrError!=IntPtr.Zero){
            var err=Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }
    public uint SpawnProcess(string program,FridaSpawnOptions options)
    {
        var spawnOptions=FridaNative.frida_spawn_options_new();
        FridaNative.frida_spawn_options_set_stdio(spawnOptions,options.Stdio);
        if (options.Argv != null)
        {
            FridaNative.frida_spawn_options_set_argv(spawnOptions, options.Argv,options.Argv.Length);
        }
        if (options.Envp != null)
        {
            FridaNative.frida_spawn_options_set_envp(spawnOptions, options.Envp,options.Envp.Length);
        }
        if (options.Env != null)
        {
            FridaNative.frida_spawn_options_set_env(spawnOptions, options.Env,options.Env.Length);
        }
        if (options.Cwd != null)
        {
            FridaNative.frida_spawn_options_set_cwd(spawnOptions, options.Cwd);
        }
        if (options.Aux != null)
        {
            FridaNative.frida_spawn_options_set_aux(spawnOptions, options.Aux.Value);
        }

        try
        {
            IntPtr ptrError=new IntPtr();
            var n=FridaNative.frida_device_spawn_sync(Handle,program,spawnOptions, IntPtr.Zero,ref ptrError);
            if(ptrError!=IntPtr.Zero){
                var err=Marshal.PtrToStructure<GError>(ptrError);
                FridaNative.g_error_free(ptrError);
                throw new FridaException(err);
            }

            return n;
        }
        finally
        {
            FridaNative.g_object_unref(spawnOptions);
        }
    }
}