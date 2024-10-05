using System.Text.Json;

namespace BloggingPlatformAPI.Helpers
{
    public class Utils
    {
        public static T? DeepClone<T>(T obj)
        {
            var serialized = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(serialized);
        }
    }
}
