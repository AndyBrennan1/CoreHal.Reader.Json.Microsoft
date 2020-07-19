using CoreHal.Reader.Loading;
using System.Collections.Generic;
using System.Text.Json;
using Validation;

namespace CoreHal.Reader.Json.Microsoft
{
    public class JsonLoader : IHalResponseLoader
    {
        public JsonSerializerOptions JsonSerializerOptions { get; private set; }

        public JsonLoader()
        {
            JsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            JsonSerializerOptions.Converters.Add(new HalResponseConvertor());
        }

        public JsonLoader(JsonSerializerOptions jsonSerializerOptions)
        {
            Requires.NotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            JsonSerializerOptions = jsonSerializerOptions;
            JsonSerializerOptions.Converters.Add(new HalResponseConvertor());
        }

        public IDictionary<string, object> Load(string rawResponse)
        {
            Requires.NotNull(rawResponse, nameof(rawResponse));

            IDictionary<string, object> result;

            if (string.IsNullOrEmpty(rawResponse))
            {
                result = new Dictionary<string, object>();
            }
            else
            {
                result = 
                    JsonSerializer.Deserialize<IDictionary<string, object>>(
                        rawResponse, this.JsonSerializerOptions);

            }

            return result;
        }
    }
}