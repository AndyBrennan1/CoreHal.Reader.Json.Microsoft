using System.Collections.Generic;
using System.Text.Json;

namespace CoreHal.Reader
{
    public class JsonDictionaryReader : IJsonDictionaryReader
    {
        public IDictionary<string, object> GetResponseDictionary(string rawResponse)
        {
            var deserializedResponse = JsonSerializer.Deserialize(rawResponse, typeof(Dictionary<string, object>));

            return deserializedResponse as Dictionary<string, object>;
        }
    }
}
