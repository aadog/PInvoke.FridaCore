using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ConsoleApp2.Frida;

public class Tools
{
    public static Dictionary<string, object> GHashTableToDictionary(IntPtr gHashTable)
    {
        var result = new Dictionary<string, object>();
        var hResult = GCHandle.Alloc(result);
        FridaNative.g_hash_table_foreach(gHashTable, static (hKey, hValue, hUserData) =>
        {
            var key = Marshal.PtrToStringUTF8(hKey)!;
            var value = GVariantToObject(hValue);
            var dict = GCHandle.FromIntPtr(hUserData).Target as Dictionary<string, object>;
            dict!.Add(key, value);
        }, GCHandle.ToIntPtr(hResult));
        hResult.Free();
        return result;
    }

    public static string GVariantGetString(IntPtr gVariant)
    {
        var value = FridaNative.g_variant_get_string(gVariant, IntPtr.Zero);
        return Marshal.PtrToStringUTF8(value)!;
    }

    public static object GVariantToObject(IntPtr gVariant)
    {
        
        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantStringType))
        {
            return GVariantGetString(gVariant);
        }

        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantInt64Type))
        {
            return FridaNative.g_variant_get_int64(gVariant);
        }

        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantInt64Type))
        {
            return FridaNative.g_variant_get_int64(gVariant);
        }

        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantBooleanType))
        {
            return FridaNative.g_variant_get_boolean(gVariant);
        }

        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantVariantType))
        {
            var v = FridaNative.g_variant_get_variant(gVariant);
            return GVariantToObject(v);
        }

        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantByteArrayType))
        {
            return FridaNative.g_variant_get_fixed_array(gVariant, IntPtr.Zero, 0);
        }

        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantVarDictType))
        {
            var result = new Dictionary<string, object>();
            var iter = FridaNative.g_variant_iter_new(gVariant);
            FridaNative.g_variant_iter_init(iter, gVariant);
            while (true)
            {
                var child = FridaNative.g_variant_iter_next_value(iter);
                if (child == 0)
                {
                    break;
                }
            
                var hKey = FridaNative.g_variant_get_child_value(gVariant, 0);
                var hValue = FridaNative.g_variant_get_child_value(gVariant, 1);
                // Console.WriteLine(GVariantGetString(hKey));
                var key=GVariantGetString(hKey);
                result[key]=GVariantToObject(hValue);
                FridaNative.g_variant_unref(hValue);
                FridaNative.g_variant_unref(hKey);
                FridaNative.g_variant_unref(child);
            }
            FridaNative.g_variant_iter_free(iter);
            return result;
        }

        if (FridaNative.g_variant_is_of_type(gVariant, FridaNative.GVariantArrayType))
        {
            var result = new List<object>();
            var iter = FridaNative.g_variant_iter_new(gVariant);
            while (true)
            {
                var child = FridaNative.g_variant_iter_next_value(iter);
                if (child == IntPtr.Zero)
                {
                    break;
                }
            
                result.Add(GVariantToObject(child));
                FridaNative.g_variant_unref(child);
            }
            
            FridaNative.g_variant_iter_free(iter);
            return result;
        }

        throw new Exception("parse variant failed");
    }

    public static List<string> GListToList(IntPtr gList)
    {
        var keys = new List<string>();
        var l = FridaNative.g_list_length(gList);
        for (var i = 0; i < l; i++)
        {
            var p = FridaNative.g_list_nth_data(gList, i);
            keys.Add(Marshal.PtrToStringAnsi(p)!);
        }

        return keys;
    }
}