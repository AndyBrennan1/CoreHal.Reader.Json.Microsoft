using System.Collections.Generic;

namespace CoreHal.Reader
{
    public interface IJsonDictionaryReader
    {
        IDictionary<string, object> GetResponseDictionary(string rawResponse);
    }
}