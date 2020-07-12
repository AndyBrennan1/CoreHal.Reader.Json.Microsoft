//using ApprovalTests;
//using ApprovalTests.Reporters;
//using System.Text.Json;
//using Xunit;

//namespace CoreHal.Reader.Json.Microsoft.Tests
//{
//    [UseReporter(typeof(DiffReporter))]
//    public class FullExamples
//    {
//        [Fact]
//        public void FullExample()
//        {
//            var jsonString = Utilities.ResourceReader.Get(Resource.ComplexExample);

//            var serializeOptions = new JsonSerializerOptions();
//            serializeOptions.Converters.Add(new Deserializer());
//            serializeOptions.WriteIndented = true;

//            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

//            Approvals.Verify(result);
//        }
//    }
//}
