using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreHal.Reader.Json.Microsoft
{
    public class HalResponseConvertor : JsonConverter<IDictionary<string,object>>
    {
        private const string LinkHRefKey = "href";
        private const string LinkTitleKey = "title";

        private static readonly Dictionary<Guid, ObjectTracker> objectCreationTracker = new Dictionary<Guid, ObjectTracker>();

        public override IDictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var responseData = new Dictionary<string, object>();
            
            string thisPropertyName = string.Empty;

            var newObjectTrackerId = Guid.NewGuid();
            objectCreationTracker.Add(newObjectTrackerId, new ObjectTracker(true, false));

            while (reader.Read())
            {
                ProcessJsonElement(
                    newObjectTrackerId,
                    ref reader,
                    responseData,
                    ref thisPropertyName);
            }

            //ProcessLinks(halReader);
            //ProcessEmbeddedItesmAndSets(halReader);

            return responseData;
        }

        //private static void ProcessEmbeddedItesmAndSets(HalReader halReader)
        //{
        //    if (halReader.Properties.ContainsKey(EmbeddedItemsKey))
        //    {
        //        var embeddedItemDictionary = (Dictionary<string, object>)halReader.Properties[EmbeddedItemsKey];

        //        foreach (var keyValuePair in embeddedItemDictionary)
        //        {
        //            if (keyValuePair.Value is IDictionary)
        //            {
        //                ProcessSingularEmbededItem(halReader, keyValuePair);
        //            }
        //            else
        //            {
        //                ProcessEmbeddedItemSet(halReader, keyValuePair);
        //            }
        //        }

        //        halReader.Properties.Remove(EmbeddedItemsKey);
        //    }
        //}

        //private static void ProcessEmbeddedItemSet(HalReader halReader, KeyValuePair<string, object> keyValuePair)
        //{
        //    var embededItemSetItems = (IEnumerable<Dictionary<string, object>>)keyValuePair.Value;

        //    var embeddedItemSet = new List<HalReader>();

        //    foreach (var embeddedItem in embededItemSetItems)
        //    {
        //        var embeddedItemHalReader = new HalReader
        //        {
        //            Properties = embeddedItem
        //        };

        //        ProcessLinks(embeddedItemHalReader);
        //        ProcessEmbeddedItesmAndSets(embeddedItemHalReader);
        //        embeddedItemSet.Add(embeddedItemHalReader);
        //    }

        //    halReader.EmbeddedItems.Add(
        //        keyValuePair.Key,
        //        new List<HalReader>(embeddedItemSet));
        //}

        //private static void ProcessSingularEmbededItem(HalReader halReader, KeyValuePair<string, object> keyValuePair)
        //{
        //    var embeddedItemHalReader = new HalReader
        //    {
        //        Properties = (Dictionary<string, object>)keyValuePair.Value
        //    };

        //    ProcessLinks(embeddedItemHalReader);
        //    ProcessEmbeddedItesmAndSets(embeddedItemHalReader);

        //    halReader.EmbeddedItems.Add(
        //        keyValuePair.Key,
        //        new List<HalReader> { embeddedItemHalReader }
        //        );
        //}

        //private static void ProcessLinks(HalReader halReader)
        //{
        //    if (halReader.Properties.ContainsKey(LinksKey))
        //    {
        //        var linksDictionary = (Dictionary<string, object>)halReader.Properties[LinksKey];

        //        foreach (var keyValuePair in linksDictionary)
        //        {
        //            if (keyValuePair.Value is IDictionary)
        //            {
        //                var linksList = new List<Link>
        //                {
        //                    GetLink((IDictionary<string, object>)keyValuePair.Value)
        //                };

        //                halReader.Links.Add(keyValuePair.Key, linksList);
        //            }
        //            else
        //            {
        //                var linkPropertySet = (IEnumerable<Dictionary<string, object>>)keyValuePair.Value;

        //                var thisSetOfLink = new List<Link>();
        //                foreach(var linkPropertyDictionary in linkPropertySet)
        //                {
        //                    thisSetOfLink.Add(GetLink(linkPropertyDictionary));
        //                }

        //                halReader.Links.Add(keyValuePair.Key, new List<Link>(thisSetOfLink));
        //            }
        //        }

        //        halReader.Properties.Remove(LinksKey);
        //    }
        //}

        //private static Link GetLink(IDictionary<string, object> linkProperties)
        //{
        //    var href = linkProperties[LinkHRefKey].ToString();

        //    var title = linkProperties.ContainsKey(LinkTitleKey)
        //                ? linkProperties[LinkTitleKey].ToString()
        //                : null;

        //    var link = new Link(href, title);

        //    return link;
        //}

        private static void ProcessJsonElement(
            Guid previousObjectTrackerId,
            ref Utf8JsonReader reader, 
            IDictionary<string, object> properties,
            ref string thisPropertyName,
            bool isArrayItem = false)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.None:
                    throw new NotImplementedException();
                case JsonTokenType.StartObject:
                    AddObject(ref reader, properties, thisPropertyName, isArrayItem);
                    break;
                case JsonTokenType.EndObject:
                    objectCreationTracker[previousObjectTrackerId].Finished = true;
                    break;
                case JsonTokenType.StartArray:
                    AddObjectCollection(ref reader, properties, thisPropertyName);
                    break;
                case JsonTokenType.EndArray:
                    objectCreationTracker[previousObjectTrackerId].Finished = true;
                    break;
                case JsonTokenType.PropertyName:
                    thisPropertyName = reader.GetString();
                    break;
                case JsonTokenType.Comment:
                    throw new NotImplementedException();
                case JsonTokenType.String:
                    AddStringBasedValue(reader, properties, thisPropertyName);
                    break;
                case JsonTokenType.Number:
                    AddNumericValue(reader, properties, thisPropertyName);
                    break;
                case JsonTokenType.True:
                    properties.Add(thisPropertyName, true);
                    break;
                case JsonTokenType.False:
                    properties.Add(thisPropertyName, false);
                    break;
                case JsonTokenType.Null:
                    properties.Add(thisPropertyName, null);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void AddObjectCollection(
            ref Utf8JsonReader reader, 
            IDictionary<string, object> properties, 
            string thisPropertyName)
        {
            var newObjectTrackerId = Guid.NewGuid();
            objectCreationTracker.Add(newObjectTrackerId, new ObjectTracker(true, false));

            var linkRelSet = new List<Dictionary<string, object>>();

            while (ObjectPropertiesStillBeingAdded(newObjectTrackerId))
            {
                reader.Read();
                var arrayItem = new Dictionary<string, object>();
                ProcessJsonElement(newObjectTrackerId, ref reader, arrayItem, ref thisPropertyName, true);

                if(objectCreationTracker[newObjectTrackerId].Finished == false)
                    linkRelSet.Add(arrayItem);
            }

            properties.Add(thisPropertyName, linkRelSet);
        }

        private static void AddObject(
            ref Utf8JsonReader reader, 
            IDictionary<string, object> properties, 
            string thisPropertyName,
            bool isArrayItem = false)
        {
            var newObjectTrackerId = Guid.NewGuid();
            objectCreationTracker.Add(newObjectTrackerId, new ObjectTracker(true, false));
            ProcessDataProperties(newObjectTrackerId, ref reader, properties, ref thisPropertyName, isArrayItem);
        }

        private static void ProcessDataProperties(
            Guid objectTrackerId,
            ref Utf8JsonReader reader, 
            IDictionary<string, object> properties,
            ref string thisPropertyName,
            bool isArrayItem = false)
        {
            if(isArrayItem)
            {
                while (ObjectPropertiesStillBeingAdded(objectTrackerId))
                {
                    reader.Read();
                    ProcessJsonElement(objectTrackerId, ref reader, properties, ref thisPropertyName);
                }
            }
            else
            {
                var propertiesDictionary = new Dictionary<string, object>();

                properties.Add(thisPropertyName, propertiesDictionary);

                while (ObjectPropertiesStillBeingAdded(objectTrackerId))
                {
                    reader.Read();
                    ProcessJsonElement(objectTrackerId, ref reader, propertiesDictionary, ref thisPropertyName);
                }
            }
        }

        private static bool ObjectPropertiesStillBeingAdded(Guid objectTrackerId)
        {
            return 
                objectCreationTracker[objectTrackerId].Started == true 
                && 
                objectCreationTracker[objectTrackerId].Finished == false;
        }

        private static void AddStringBasedValue(
            Utf8JsonReader reader, 
            IDictionary<string, object> properties,
            string thisPropertyName)
        {
            var stringResult = reader.GetString();

            if (DateTime.TryParse(stringResult, out DateTime dateResult))
            {
                properties.Add(thisPropertyName, dateResult);
            }
            else if (Guid.TryParse(stringResult, out Guid guidResult))
            {
                properties.Add(thisPropertyName, guidResult);
            }
            else
            {
                properties.Add(thisPropertyName, stringResult);
            }
        }

        private static Utf8JsonReader AddNumericValue(
            Utf8JsonReader reader, 
            IDictionary<string, object> properties, 
            string thisPropertyName)
        {
            if (reader.TryGetDouble(out double numberAsDouble))
            {
                properties.Add(thisPropertyName, numberAsDouble);
            }
            else
            {
                throw new Exception("Number cannot be stored as a double.");
            }

            return reader;
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<string, object> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        private class ObjectTracker
        {
            public ObjectTracker(bool started, bool finished)
            {
                Started = started;
                Finished = finished;
            }

            public bool Started { get; set; }
            public bool Finished { get; set; }
        }
    }
}