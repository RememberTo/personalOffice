using Newtonsoft.Json;
using System.Text;

namespace MessageBus.Common
{
    public class SerializeHelper
    {
        internal static async Task<string> JsonSerizlizeAsync<T>(T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj);

            return await
                Task.Factory.StartNew(() => JsonSerizlize(obj)) ?? throw new InvalidOperationException();
        }

        internal static string JsonSerizlize<T>(T obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj);

            return JsonConvert.SerializeObject(obj) ?? throw new InvalidOperationException();
        }

        internal static async Task<T> JsonDeserizlizeAsync<T>(string json) where T : class
        {
            ArgumentNullException.ThrowIfNull(json);
            return await 
                Task.Factory.StartNew(() => JsonDeserizlize<T>(json)) ?? throw new InvalidOperationException();

        }

        internal static T JsonDeserizlize<T>(string obj) where T : class
        {
            ArgumentNullException.ThrowIfNull(obj);

            return JsonConvert.DeserializeObject<T>(obj) ?? throw new InvalidOperationException();
        }

        internal static string GetJsonFromByte(byte[] obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            return Encoding.UTF8.GetString(obj);
        }
    }
}
