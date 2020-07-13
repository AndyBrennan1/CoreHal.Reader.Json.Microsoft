using DeepEqual.Syntax;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace CoreHal.Reader.Json.Microsoft.Tests
{
    public class HalResponseConvertorTests
    {
        [Fact]
        public void Reading_EmptyJsonString_ReturnsEmptyHalReader()
        {
            var jsonString = "{ }";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.IsAssignableFrom<IDictionary<string, object>>(result);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Reading_ContentWithStringProperty_AddsPropertyToResult()
        {
            var jsonString = "{ \"string-Property\": \"test value\" }";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["string-Property"]);
            Assert.IsType<string>(result["string-Property"]);
            Assert.Equal(expected: "test value", actual: result["string-Property"]);
        }

        [Fact]
        public void Reading_ContentWithTwoStringProperties_AddsPropertiesToResult()
        {
            var jsonString = "{ " +
                                "\"string-Property\": \"test value\", " +
                                "\"string-Property2\": \"test value2\" " +
                             "}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.IsType<string>(result["string-Property"]);
            Assert.IsType<string>(result["string-Property2"]);
            Assert.Equal(expected: 2, actual: result.Count);
            Assert.Equal(expected: "test value", actual: result["string-Property"]);
            Assert.Equal(expected: "test value2", actual: result["string-Property2"]);
        }

        [Fact]
        public void Reading_ContentWithIntegerProperty_AddsPropertyToResultAsDouble()
        {
            var jsonString = "{ \"integer-Property\": 123 }";

            const double expectedResult = 123;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["integer-Property"]);
            Assert.IsType<double>(result["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithInt64PropertyOfMaxValue_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {Int64.MaxValue} }}";

            double expectedResult = Int64.MaxValue;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["integer-Property"]);
            Assert.IsType<double>(result["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithInt64PropertyOfMinValue_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {Int64.MinValue} }}";

            double expectedResult = Int64.MinValue;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["integer-Property"]);
            Assert.IsType<double>(result["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithDecimalProperty_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {123.23M} }}";

            double expectedResult = (double)123.23M;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["integer-Property"]);
            Assert.IsType<double>(result["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithDoubleProperty_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {double.MaxValue} }}";

            double expectedResult = double.MaxValue;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["integer-Property"]);
            Assert.IsType<double>(result["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithFloatProperty_AddsPropertyToResultRoundedAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {999.999F} }}";

            const double expectedResult = 999.999F;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["integer-Property"]);
            Assert.IsType<double>(result["integer-Property"]);
            Assert.Equal(expected: Math.Round(expectedResult, 3), actual: result["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithDateProperty_AddsPropertyToResult()
        {
            var thisDate = new DateTime(2020, 7, 9, 10, 51, 12);

            var jsonString = $"{{ \"date-Property\": \"2020-07-09T10:51:12\" }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["date-Property"]);
            Assert.IsType<DateTime>(result["date-Property"]);
            Assert.Equal(expected: thisDate, actual: (DateTime)result["date-Property"]);
        }

        [Fact]
        public void Reading_ContentWithGuidProperty_AddsPropertyToResult()
        {
            var thisGuid = Guid.Parse("1b935602-b202-4246-b48b-8e17291e5376");

            var jsonString = $"{{ \"guid-Property\": \"1b935602-b202-4246-b48b-8e17291e5376\" }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["guid-Property"]);
            Assert.IsType<Guid>(result["guid-Property"]);
            Assert.Equal(expected: thisGuid, actual: Guid.Parse(result["guid-Property"].ToString()));
        }

        [Fact]
        public void Reading_ContentWithBooleanTrueProperty_AddsPropertyToResult()
        {
            var thisBoolean = true;

            var jsonString = $"{{ \"boolean-Property\": true }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["boolean-Property"]);
            Assert.IsType<bool>(result["boolean-Property"]);
            Assert.Equal(expected: thisBoolean, actual: bool.Parse(result["boolean-Property"].ToString()));
        }

        [Fact]
        public void Reading_ContentWithBooleanFalseProperty_AddsPropertyToResult()
        {
            var thisBoolean = false;

            var jsonString = $"{{ \"boolean-Property\": false }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.NotNull(result["boolean-Property"]);
            Assert.IsType<bool>(result["boolean-Property"]);
            Assert.Equal(expected: thisBoolean, actual: bool.Parse(result["boolean-Property"].ToString()));
        }

        [Fact]
        public void Reading_ContentWithNullProperty_AddsPropertyToResult()
        {
            var jsonString = $"{{ \"some-Property\": null }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Null(result["some-Property"]);
        }

        [Fact]
        public void Reading_ContentWithComplexProperty_AddsPropertyToResult()
        {
            var jsonString = $"{{ " +
                                    $"\"complex-Property\": " +
                                        $"{{ " +
                                            $"\"property1\": \"hello\"," +
                                            $"\"property2\": \"world\" " +
                                        $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "property1", "hello" },
                { "property2", "world" }
            };

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expected: expectedDictionary, actual: result["complex-Property"]);
        }

        [Fact]
        public void Reading_ContentWithComplexPropertyHavingComplexProperty_AddsPropertyToResult()
        {
            var jsonString = $"{{ " +
                                    $"\"complex-Property\": " +
                                        $"{{ " +
                                            $"\"property1\": \"hello\"," +
                                            $"\"property2\": \"world\"," +
                                            $"\"second-complex-property\": " +
                                                $"{{ " +
                                                    $"\"propertyA\": \"bonjour\"," +
                                                    $"\"propertyB\": \"le monde\"" +
                                                $"}} " +
                                        $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "property1", "hello" },
                { "property2", "world" },
                { "second-complex-property", new Dictionary<string, object>
                    {
                        { "propertyA", "bonjour" },
                        { "propertyB", "le monde" },
                    }
                }
            };

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expected: expectedDictionary, actual: result["complex-Property"]);
        }

        [Fact]
        public void Reading_ContentWithComplexPropertyAndSimpleOnes_AddsPropertiesToResult()
        {
            var jsonString = $"{{ " +
                                    $"\"story\": \"once\"," +
                                    $"\"start\": \"upon a time\"," +
                                    $"\"complex-Property\": " +
                                        $"{{ " +
                                            $"\"property1\": \"hello\"," +
                                            $"\"property2\": \"world\"," +
                                            $"\"second-complex-property\": " +
                                                $"{{ " +
                                                    $"\"propertyA\": \"bonjour\"," +
                                                    $"\"propertyB\": \"le monde\"" +
                                                $"}} " +
                                        $"}}, " +
                                    $"\"end\": \"the end\"" +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "property1", "hello" },
                { "property2", "world" },
                { "second-complex-property", new Dictionary<string, object>
                    {
                        { "propertyA", "bonjour" },
                        { "propertyB", "le monde" },
                    }
                }
            };

            Assert.Equal(expected: "once", actual: result["story"].ToString());
            Assert.Equal(expected: "upon a time", actual: result["start"].ToString());
            Assert.Equal(expected: expectedDictionary, actual: result["complex-Property"]);
            Assert.Equal(expected: "the end", actual: result["end"].ToString());
        }

        [Fact]
        public void Reading_ContentWithRelAndOneLink_AddsTheLinkToTheLinksCollection()
        {
            var jsonString = $"{{ " +
                                $"\"_links\": " +
                                    $"{{ " +
                                        $"\"self\": " +
                                            $"{{ " +
                                                $"\"href\": \"/api/categories\"," +
                                                $"\"title\": \"Some Title\"," +
                                                $"\"templated\": false" +
                                            $"}} " +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                {
                    "_links",
                    new Dictionary<string, IDictionary<string, object>>
                    {
                        {
                            "self",
                            new Dictionary<string, object>
                            {
                                { "href", "/api/categories" },
                                { "title", "Some Title" },
                                { "templated", false }
                            }
                        }
                    }
                }
            };

            expectedDictionary.ShouldDeepEqual(result);
        }

        [Fact]
        public void Reading_ContentWithTwoRelsEachWithLink_AddsTheLinksToTheLinksCollection()
        {
            var jsonString = $"{{ " +
                                $"\"_links\": " +
                                    $"{{ " +
                                        $"\"self\": " +
                                            $"{{ " +
                                                $"\"href\": \"/api/categories/123\"," +
                                                $"\"title\": \"Category\"," +
                                                $"\"templated\": false" +
                                            $"}}," +
                                        $"\"parent\": " +
                                            $"{{ " +
                                                $"\"href\": \"/api/categories\"," +
                                                $"\"title\": \"Categories\"" +
                                            $"}} " +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                {
                    "_links",
                    new Dictionary<string, IDictionary<string, object>>
                    {
                        {
                            "self",
                            new Dictionary<string, object>
                            {
                                { "href", "/api/categories/123" },
                                { "title", "Category" },
                                { "templated", false }
                            }
                        },
                         {
                            "parent",
                            new Dictionary<string, object>
                            {
                                { "href", "/api/categories" },
                                { "title", "Categories" }
                            }
                        }
                    }
                }
            };

            expectedDictionary.ShouldDeepEqual(result);
        }

        [Fact]
        public void Reading_ContentWithRelWithTwoLinks_AddsTheLinksToTheLinksCollection()
        {
            var jsonString = $"{{ " +
                                $"\"_links\": " +
                                    $"{{ " +
                                        $"\"orders\": " +
                                            $"[" +
                                                $"{{ " +
                                                    $"\"href\": \"/api/orders/123\"," +
                                                    $"\"title\": \"Order\"" +
                                                $"}}, " +
                                                $"{{ " +
                                                    $"\"href\": \"/api/orders/456\"," +
                                                    $"\"title\": \"Order\"" +
                                                $"}} " +
                                            $"]" +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                {
                    "_links",
                    new Dictionary<string, IEnumerable<IDictionary<string, object>>>
                    {
                        {
                            "orders",
                            new List<Dictionary<string, object>>
                            {
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/orders/123" },
                                        { "title", "Order" }
                                    }
                                },
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "href", "/api/orders/456" },
                                        { "title", "Order" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            expectedDictionary.ShouldDeepEqual(result);
        }

        [Fact]
        public void Reading_ContentWithEmbeddedItem_AddsTheEmbeddedItemToTheEmbeddedItemsCollection()
        {
            var jsonString = $"{{ " +
                                $"\"_embedded\": " +
                                    $"{{ " +
                                        $"\"something\": " +
                                            $"{{ " +
                                                $"\"string-Property\": \"Some string\"," +
                                                $"\"integer-Property\": 123" +
                                            $"}} " +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "_embedded", new Dictionary<string, object>
                    {
                        {
                            "something", new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            }
                        }
                    }
                }
            };

            Assert.NotNull(result);
            Assert.Equal(expected: 1, actual: result.Count);
            Assert.Equal(expected: 1, actual: ((IDictionary<string, object>)result["_embedded"]).Count);
            expectedDictionary.ShouldDeepEqual(result);
        }

        [Fact]
        public void Reading_ContentWithTwoDifferentEmbeddedItems_AddsBothEmbeddedItemsToTheEmbeddedItemsCollection()
        {
            var jsonString = $"{{ " +
                                $"\"_embedded\": " +
                                    $"{{ " +
                                        $"\"something\": " +
                                            $"{{ " +
                                                $"\"string-Property\": \"Some string\"," +
                                                $"\"integer-Property\": 123" +
                                            $"}}, " +
                                        $"\"something-Else\": " +
                                            $"{{ " +
                                                $"\"bool-Property\": true, " +
                                                $"\"date-Property\": \"2020-07-09T10:51:12\"" +
                                            $"}} " +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, IDictionary<string, object>>
                    {
                        {
                            "something",
                            new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123 }
                            }
                        },
                         {
                            "something-Else",
                            new Dictionary<string, object>
                            {
                                { "bool-Property", true },
                                { "date-Property", new DateTime(2020, 7, 9, 10, 51, 12)}
                            }
                        }
                    }
                }
            };

            Assert.NotNull(result);
            Assert.Equal(expected: 1, actual: result.Count);
            Assert.Equal(expected: 2, actual: ((IDictionary<string, object>)result["_embedded"]).Count);
            expectedDictionary.ShouldDeepEqual(result);
        }

        [Fact]
        public void Reading_ContentWithTwoEmbeddedItemsInSet_AddsBothEmbeddedItemsToTheEmbeddedItemsCollection()
        {
            var jsonString = $"{{ " +
                                $"\"_embedded\": " +
                                    $"{{ " +
                                        $"\"something\": " +
                                            $"[" +
                                                $"{{ " +
                                                    $"\"string-Property\": \"Some string\"," +
                                                    $"\"integer-Property\": 123" +
                                                $"}}," +
                                                $"{{ " +
                                                    $"\"string-Property\": \"Some different string\"," +
                                                    $"\"integer-Property\": 321" +
                                                $"}}" +
                                            $"]" +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                {
                    "_embedded",
                    new Dictionary<string, IEnumerable<IDictionary<string, object>>>
                    {
                        {
                            "something",
                            new List<Dictionary<string, object>>
                            {
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "string-Property", "Some string" },
                                        { "integer-Property", 123 }
                                    }
                                },
                                {
                                    new Dictionary<string, object>
                                    {
                                        { "string-Property", "Some different string" },
                                        { "integer-Property", 321 }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var embeddedItemDictionary = (IDictionary<string, object>)result["_embedded"];
            var embeddedItemSetDictionary = (List<Dictionary<string, object>>)embeddedItemDictionary["something"];

            Assert.NotNull(result);
            Assert.Equal(expected: 1, actual: result.Count);
            Assert.Equal(expected: 1, actual: embeddedItemDictionary.Count);
            Assert.Equal(expected: 2, actual: embeddedItemSetDictionary.Count);
            expectedDictionary.ShouldDeepEqual(result);
        }

        [Fact]
        public void Reading_ContentWithEmbeddedItemContainingOneLink_AddsTheEmbeddedItemToTheEmbeddedItemsCollection()
        {
            var jsonString = $"{{ " +
                                $"\"_embedded\": " +
                                    $"{{ " +
                                        $"\"something\": " +
                                            $"{{ " +
                                                $"\"_links\": " +
                                                $"{{ " +
                                                    $"\"self\": " +
                                                        $"{{ " +
                                                            $"\"href\": \"/api/categories\"," +
                                                            $"\"title\": \"Some Title\"," +
                                                            $"\"templated\": false" +
                                                        $"}} " +
                                                $"}}, " +
                                                $"\"string-Property\": \"Some string\"," +
                                                $"\"integer-Property\": 123" +
                                            $"}}" +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "_embedded", new Dictionary<string, object>
                    {
                        {
                            "something", new Dictionary<string, object>
                            {
                                {
                                    "_links",
                                    new Dictionary<string, IDictionary<string, object>>
                                    {
                                        {
                                            "self",
                                            new Dictionary<string, object>
                                            {
                                                { "href", "/api/categories" },
                                                { "title", "Some Title" },
                                                { "templated", false }
                                            }
                                        }
                                    }
                                },
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            }
                        }
                    }
                }
            };

            expectedDictionary.ShouldDeepEqual(result);
        }

        [Fact]
        public void Reading_ContentWithEmbeddedItemContaininEmbeddedItem_AddsTheEmbeddedItemAndItsOwnEmbeddedItemToResult()
        {
            var jsonString = $"{{ " +
                                $"\"_embedded\": " +
                                    $"{{ " +
                                        $"\"something\": " +
                                            $"{{ " +
                                                $"\"string-Property\": \"Some string\"," +
                                                $"\"integer-Property\": 123," +
                                                $"\"_embedded\": " +
                                                    $"{{ " +
                                                        $"\"embedded-in-embedded-item\": " +
                                                            $"{{ " +
                                                                $"\"bool-Property\": true" +
                                                            $"}}" +
                                                    $"}} " +
                                            $"}}" +
                                    $"}} " +
                            $"}}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new HalResponseConvertor());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<IDictionary<string,object>>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "_embedded", new Dictionary<string, object>
                    {
                        {
                            "something", new Dictionary<string, object>
                            {

                                { "string-Property", "Some string" },
                                { "integer-Property", 123},
                                {
                                    "_embedded",
                                    new Dictionary<string, IDictionary<string, object>>
                                    {
                                        {
                                            "embedded-in-embedded-item",
                                            new Dictionary<string, object>
                                            {
                                                { "bool-Property",true }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            expectedDictionary.ShouldDeepEqual(result);
        }
    }
}