using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreHal.Reader.Json.Microsoft
{
    public class HalResponseConvertor : JsonConverter<IDictionary<string,object>>
    {
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

            return responseData;
        }

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