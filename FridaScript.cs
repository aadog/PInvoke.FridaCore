using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading.Channels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PInvoke.FridaCore;

public class FridaScript(IntPtr handle) : IFridaObject
{
    public IntPtr Handle { get; set; } = handle;
    public ConcurrentDictionary<string, Channel<JArray>> RpcDictionary = new();
    public FridaNative.ScriptOnMessageCallback OnRpcCallback;
    public JArray RpcCall(string callId,string methodName, List<object> args,CancellationToken token)
    {
        CancellationTokenSource cancellationTokenSource=CancellationTokenSource.CreateLinkedTokenSource(token);
        var cancelToken = cancellationTokenSource.Token;
        var jRpcString = JsonConvert.SerializeObject(JArray.FromObject(new List<object>
        {
            "frida:rpc",
            callId,
            "call",
            methodName,
            args
        }));
        Channel<JArray> channel = Channel.CreateBounded<JArray>(1);
        RpcDictionary.AddOrUpdate(callId, channel, (a,b) => channel);
        Post(jRpcString,IntPtr.Zero);
        JArray arrayResult=null;
        Task.Run(async () =>
        {
            try
            {
                var c = await channel.Reader.WaitToReadAsync(cancelToken);
                var retArray = await channel.Reader.ReadAsync();
                arrayResult = retArray;
                retArray=null;
                cancellationTokenSource.Cancel();
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine("aaa");
                throw;
            }
        });
        
        cancelToken.WaitHandle.WaitOne();
        return arrayResult;
    }

    public void OnRpcCall(IntPtr script, string message, IntPtr data, IntPtr userData)
    {
        var jsonMessage = JsonConvert.DeserializeObject<JObject>(message);
        var messageType = jsonMessage["type"].ToString();
        switch (messageType)
        {
            case "send":
            {
                if (jsonMessage["payload"].Type == JTokenType.Array)
                {
                    var payload = jsonMessage["payload"].ToObject<JArray>();
                    if (payload.Count == 4)
                    {
                        if (payload[0].ToString() == "frida:rpc")
                        {
                            var callId = payload[1].ToObject<string>();
                            var callStatus = payload[2].ToString();
                            var callReturn = payload[3].ToString();
                                
                            Channel<JArray> channel;
                            RpcDictionary.TryRemove(callId, out channel);
                            if (channel != null)
                            {
                                // Console.WriteLine($"callId:{callId},调用返回:{callStatus},return:{callReturn}");
                                channel.Writer.TryWrite(payload);
                            }
                        }
                    }
                }
                break;
            }
            default:
            {
                // Console.WriteLine(jsonMessage.ToString());
                break;
            }
        }
    }
    
    public void InstallRpcHandle()
    {
        OnRpcCallback = OnRpcCall;
        OnMessage(OnRpcCallback);
    }

    public void Load()
    {
        IntPtr ptrError = new IntPtr();
        FridaNative.frida_script_load_sync(Handle, IntPtr.Zero, ref ptrError);
        if (ptrError != IntPtr.Zero)
        {
            var err = Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }

    public void UnLoad()
    {
        IntPtr ptrError = new IntPtr();
        FridaNative.frida_script_unload_sync(Handle, IntPtr.Zero, ref ptrError);
        if (ptrError != IntPtr.Zero)
        {
            var err = Marshal.PtrToStructure<GError>(ptrError);
            FridaNative.g_error_free(ptrError);
            throw new FridaException(err);
        }
    }

    public bool IsDestroyed()
    {
        return FridaNative.frida_script_is_destroyed(Handle);
    }

    public void Post(string json, IntPtr data)
    {
        FridaNative.frida_script_post(Handle,json, data);
    }
    
    public void OnMessage(FridaNative.ScriptOnMessageCallback callback)
    {
        On("message", Marshal.GetFunctionPointerForDelegate(callback));
    }

    public void OnDestroyed(FridaNative.GCallback callback)
    {
        On("destroyed", Marshal.GetFunctionPointerForDelegate(callback));
    }

    public void On(string signal, IntPtr callback)
    {
        FridaNative.g_signal_connect_data(Handle, signal, callback, IntPtr.Zero, IntPtr.Zero,
            GConnectFlags.G_CONNECT_DEFAULT);
    }

}