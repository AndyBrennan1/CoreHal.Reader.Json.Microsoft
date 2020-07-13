using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace CoreHal.Reader.Json.Microsoft.Tests
{
    public class JsonLoaderTests
    {
        [Fact]
        public void Constructing_WithNothingProvided_CreatesDefaultSettings()
        {
            var loader = new JsonLoader();

            Assert.NotNull(loader.JsonSerializerOptions);
        }

        [Fact]
        public void Constructing_WithNothingProvided_SetsWriteIndentedToTrue()
        {
            var loader = new JsonLoader();

            Assert.True(loader.JsonSerializerOptions.WriteIndented);
        }

        [Fact]
        public void Constructing_WithNothingProvided_MakesHalResponseConvertorAvailable()
        {
            var loader = new JsonLoader();

            var convertorLoaded = 
                loader.JsonSerializerOptions.Converters.Any(
                    convertor => convertor.GetType() == typeof(HalResponseConvertor));

            Assert.True(convertorLoaded);
        }

        [Fact]
        public void Constructing_WithNullJsonSerializerOptionsProvided_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new JsonLoader(null);
            });
        }

        [Fact]
        public void Constructing_WithJsonSerializerOptionsProvided_SetsTheOptionsToUseToTheInstanceProvided()
        {
            var serializerOptions = new JsonSerializerOptions();

            var loader = new JsonLoader(serializerOptions);

            var convertorLoaded =
                loader.JsonSerializerOptions.Converters.Any(
                    convertor => convertor.GetType() == typeof(HalResponseConvertor));

            Assert.Same(serializerOptions, loader.JsonSerializerOptions);
        }

        [Fact]
        public void Constructing_WithJsonSerializerOptionsProvided_MakesHalResponseConvertorAvailable()
        {
            var serializerOptions = new JsonSerializerOptions();

            var loader = new JsonLoader(serializerOptions);

            var convertorLoaded =
                loader.JsonSerializerOptions.Converters.Any(
                    convertor => convertor.GetType() == typeof(HalResponseConvertor));

            Assert.True(convertorLoaded);
        }

        [Fact]
        public void Loading_WithNullResponseData_ThrowsException()
        {
            string rawResponseData = null;

            var loader = new JsonLoader();

            Assert.Throws<ArgumentNullException>(() =>
            {
                loader.Load(rawResponseData);
            });
        }

        [Fact]
        public void Loading_WithEmptyResponseData_ReturnsEmptyDictionary()
        {
            var rawResponseData = string.Empty;

            var loader = new JsonLoader();

            var result = loader.Load(rawResponseData);

            Assert.Empty(result);
        }

        [Fact]
        public void Loading_WithResponseData_ReturnsPopulatedDictionary()
        {
            var rawResponseData = "{ \"some-Property\": \"Some Value\" }";

            var loader = new JsonLoader();

            var result = loader.Load(rawResponseData);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "some-Property", "Some Value" }
            };

            Assert.Equal(expected: expectedDictionary, actual: result);
        }
    }
}