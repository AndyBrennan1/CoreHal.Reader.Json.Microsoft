//using Moq;
//using System;
//using System.Collections.Generic;
//using Xunit;

//namespace CoreHal.Reader.Tests
//{
//    public class HalReaderTests
//    {
//        [Fact]
//        public void Constructing_WithNullJsonDictionaryReaderProvided_ThrowsException()
//        {
//            IJsonDictionaryReader reader = null;

//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                new HalReader(reader);
//            });
//        }

//        [Fact]
//        public void Populating_WithNullRawResultProvided_ThrowsException()
//        {
//            var responseReader = new Mock<IJsonDictionaryReader>();
//            string rawResult = null;

//            var reader = new HalReader(responseReader.Object);
            
//            Assert.Throws<ArgumentNullException>(() =>
//            {
//                reader.Populate(rawResult);
//            });
//        }

//        [Fact]
//        public void Populating_WithEmptyRawResultProvided_ThrowsException()
//        {
//            var responseReader = new Mock<IJsonDictionaryReader>();
//            string rawResult = string.Empty;

//            var reader = new HalReader(responseReader.Object);

//            Assert.Throws<ArgumentException>(() =>
//            {
//                reader.Populate(rawResult);
//            });
//        }

//        [Fact]
//        public void Populating_WithRawResultProvided_StoresItInTheRawResultProperty()
//        {
//            var responseReader = new Mock<IJsonDictionaryReader>();
//            string rawResult = $"{{\"some-property\": \"some value\" }}";

//            var reader = new HalReader(responseReader.Object);

//            reader.Populate(rawResult);

//            Assert.Equal(expected: rawResult, actual: reader.RawResult);
//        }

//        [Fact]
//        public void Populating_WithRawResultProvided_PopulatesResultDictionary()
//        {
//            string rawResult = $"{{\"some-property\": \"some value\" }}";

//            var expectedDictionary = new Dictionary<string, object>
//            {
//                { "some-property", "some value" }
//            };

//            var responseReader = new Mock<IJsonDictionaryReader>();
//            responseReader.Setup(reader => reader.GetResponseDictionary(rawResult)).Returns(expectedDictionary);

//            var reader = new HalReader(responseReader.Object);

//            reader.Populate(rawResult);

//            Assert.NotNull(reader.ResultDictionary);
//        }


//        //[Fact]
//        //public void Loading_WithSimplePropertyInfoProvided_PopulatesProperties()
//        //{
//        //    var loader = new MockLoader();

//        //    var resource = new HalResource(loader);
//        //    resource.Load();

//        //    Assert.NotNull(resource.Properties["string-property"]);
//        //    Assert.NotNull(resource.Properties["integer-property"]);
//        //    Assert.NotNull(resource.Properties["bool-property"]);
//        //    Assert.NotNull(resource.Properties["guid-property"]);
//        //    Assert.NotNull(resource.Properties["date-property"]);

//        //    Assert.Equal(expected: "some string", actual: resource.Properties["string-property"]);
//        //    Assert.Equal(expected: 999, actual: resource.Properties["integer-property"]);
//        //    Assert.Equal(expected: true, actual: resource.Properties["bool-property"]);
//        //    Assert.Equal(expected: Guid.Parse("a2e965e6-c2fb-42e9-8d33-c3f5cf64cd60"), actual: resource.Properties["guid-property"]);
//        //    Assert.Equal(expected: new DateTime(2020, 7, 4), actual: resource.Properties["date-property"]);
//        //}

//        //[Fact]
//        //public void Loading_WithTwoLinkCollectionsProvided_Adds2LinkCollections()
//        //{
//        //    var loader = new MockLoader();

//        //    var resource = new HalResource(loader);
//        //    resource.Load();

//        //    Assert.True(resource.Links.Count == 2);
//        //}

//        //[Fact]
//        //public void Loading_WithTwoLinksInDifferentCollectionsProvided_Adds1LinkToEachCollection()
//        //{
//        //    var loader = new MockLoader();

//        //    var resource = new HalResource(loader);
//        //    resource.Load();

//        //    Assert.Single(resource.Links["rel1"]);
//        //    Assert.Single(resource.Links["rel2"]);
//        //}
//    }
//}