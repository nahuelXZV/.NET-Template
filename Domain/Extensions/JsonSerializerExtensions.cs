using System.Reflection;
using System.Text.Json;

namespace Domain.Extensions;

public static class JsonSerializerExtensions
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static T Deserialize<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    public static object Deserialize(this string json, Type type)
    {
        return JsonSerializer.Deserialize(json, type, _options);
    }

    public static string Serialize<TValue>(this TValue obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }

    /// <summary>
    /// Serializa un objeto a un string JSON, utilizando relfection para determinar el tipo de objeto y no perder la información de los tipos derivados.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Serialize(this object obj)
    {
        Type originalType = obj.GetType();

        Type extensionType = typeof(JsonSerializerExtensions);

        var method = extensionType.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(m => m.Name == nameof(Serialize) && m.IsGenericMethodDefinition)
            .FirstOrDefault();

        MethodInfo generic = method.MakeGenericMethod(originalType);
        var serialized = generic.Invoke(null, new object[] { obj });
        return (string)serialized;
    }

    public static JsonElement SerializeToElement<TValue>(this TValue obj)
    {
        return JsonSerializer.SerializeToElement(obj, _options);
    }
}

