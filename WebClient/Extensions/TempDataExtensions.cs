using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebClient.Extensions;

public static class TempDataExtensions
{
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
    {
        tempData[key] = value.Serialize();
    }
    public static T Get<T>(this ITempDataDictionary tempData, string key)
    {
        tempData.TryGetValue(key, out object o);
        return o == null ? default : ((string)o).Deserialize<T>();
    }

    public static T Peek<T>(this ITempDataDictionary tempData, string key)
    {
        var o = (string)tempData.Peek(key);
        return o == null ? default : o.Deserialize<T>();
    }
}
