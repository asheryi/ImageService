using System.Web.Script.Serialization;

namespace SharedResources.Communication
{
    public class InfoToString<T>
    {

        public static string ToJSON(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }


        public static T FromJSON(string json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(json);
        }
    }
}
