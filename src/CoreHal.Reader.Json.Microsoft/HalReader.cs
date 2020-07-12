using CoreHal.Graph;
using System;
using System.Collections.Generic;

namespace CoreHal.Reader
{
    [Serializable]
    public class HalReader
    {
        public IDictionary<string, IEnumerable<Link>> Links { get; set; }
        public IDictionary<string, object> Properties { get; set; }
        public IDictionary<string, IEnumerable<HalReader>> EmbeddedItems { get;  set; }

        public HalReader()
        {
            Links = new Dictionary<string, IEnumerable<Link>>();
            Properties = new Dictionary<string, object>();
            EmbeddedItems = new Dictionary<string, IEnumerable<HalReader>>();
        }
    }
}
