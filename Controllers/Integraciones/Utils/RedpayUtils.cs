using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace desafiocoaniquem.Controllers.Integraciones.Utils
{
    public class SignatureGenerator
    {
        public string GenerateSignature(object payload, string secret)
        {
            var sortedPayload = SortObject(payload);
            var message = "";
            foreach (var property in sortedPayload)
            {
                if (property.Key == "signature") continue;
                message += property.Key + JsonConvert.SerializeObject(property.Value);
            }

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                string signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return signature;
            }
        }

        private SortedDictionary<string, object> SortObject(object payload)
        {
            var jsonObject = JObject.FromObject(payload);
            var sortedJsonObject = JsonKeySorter.SortKeys(jsonObject);

            var dict = sortedJsonObject.ToObject<Dictionary<string, object>>();
            var sortedDict = new SortedDictionary<string, object>(dict);
            return sortedDict;
        }
    }

    public class JsonKeySorter
    {
        public static JObject SortKeys(JObject json)
        {
            if (json == null)
                return null;

            var sortedJson = new JObject();

            foreach (var key in GetKeysAlphabetical(json))
            {
                sortedJson[key] = SortJToken(json[key]);
            }

            return sortedJson;
        }

        private static IEnumerable<string> GetKeysAlphabetical(JObject json)
        {
            return json.Properties().OrderBy(p => p.Name).Select(p => p.Name);
        }

        private static JToken SortJToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return SortKeys(token as JObject);
                case JTokenType.Array:
                    return SortJArray(token as JArray);
                default:
                    return token;
            }
        }

        private static JArray SortJArray(JArray array)
        {
            var sortedArray = new JArray();
            foreach (var item in array)
            {
                sortedArray.Add(SortJToken(item));
            }

            return sortedArray;
        }
    }
}
