using System;
using System.Linq;
using Newtonsoft.Json;

namespace AirAstana.FlightControl.Application.Features.Behaviors;

public static class BehaviourExtensions
{
    private static string GetGenericTypeName(this Type type)
    {
        string typeName;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }

        return typeName;
    }

    public static string GetGenericTypeName(this object @object)
    {
        return @object.GetType().GetGenericTypeName();
    }

    public static string GetInJson(this object @object)
    {
        return JsonConvert.SerializeObject(@object,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, });
    }
}