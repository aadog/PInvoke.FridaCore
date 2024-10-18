namespace PInvoke.FridaCore;

public class FridaMainLoop(IntPtr handle)
{
    public IntPtr Handle { get; set; } = handle;

    public bool IsRunning()
    {
        return FridaNative.g_main_loop_is_running(Handle);
    }

    public void Run()
    {
        FridaNative.g_main_loop_run(Handle);
    }
    public void MainLoopUnRef()
    {
        FridaNative.g_main_loop_unref(Handle);
    }
}