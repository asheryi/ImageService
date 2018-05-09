

using Newtonsoft.Json;

namespace SharedResources
{
    // if in the future we would like to switch JSON to another serializaton mechanism , this will be the only place to address.
    public class ObjectConverter
    {
        public static string Serialize(object ser)
        {
            return JsonConvert.SerializeObject(ser);
        }

        public static T Deserialize<T>(string des)
        {
            return JsonConvert.DeserializeObject<T>(des);
        }
    }
}
