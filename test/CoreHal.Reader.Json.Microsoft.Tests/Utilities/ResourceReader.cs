using System.IO;

namespace CoreHal.Reader.Json.Microsoft.Tests.Utilities
{
    public static class ResourceReader
    {
        public static string Get(byte[] resourceContentByteArray)
        {
            MemoryStream MS = new MemoryStream(resourceContentByteArray);
            StreamReader sr = new StreamReader(MS);

            return sr.ReadToEnd();
        }
    }
}