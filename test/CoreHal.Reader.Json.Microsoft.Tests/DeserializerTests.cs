using CoreHal.Graph;
using DeepEqual.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace CoreHal.Reader.Json.Microsoft.Tests
{
    public class DeserializerTests
    {
        [Fact]
        public void Reading_EmptyJsonString_ReturnsEmptyHalReader()
        {
            var jsonString = "{ }";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;
           
            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.IsType<HalReader>(result);
            Assert.NotNull(result);
            Assert.NotNull(result.EmbeddedItems);
            Assert.NotNull(result.Links);
            Assert.NotNull(result.Properties);
            Assert.Empty(result.EmbeddedItems);
            Assert.Empty(result.Links);
            Assert.Empty(result.Properties);
        }

        [Fact]
        public void Reading_ContentWithStringProperty_AddsPropertyToResult()
        {
            var jsonString = "{ \"string-Property\": \"test value\" }";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["string-Property"]);
            Assert.IsType<string>(result.Properties["string-Property"]);
            Assert.Equal(expected: "test value", actual: result.Properties["string-Property"]);
        }

        [Fact]
        public void Reading_ContentWithTwoStringProperties_AddsPropertiesToResult()
        {
            var jsonString = "{ " +
                                "\"string-Property\": \"test value\", " +
                                "\"string-Property2\": \"test value2\" " +
                             "}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.IsType<string>(result.Properties["string-Property"]);
            Assert.IsType<string>(result.Properties["string-Property2"]);
            Assert.Equal(expected: 2, actual: result.Properties.Count);
            Assert.Equal(expected: "test value", actual: result.Properties["string-Property"]);
            Assert.Equal(expected: "test value2", actual: result.Properties["string-Property2"]);
        }

        [Fact]
        public void Reading_ContentWithIntegerProperty_AddsPropertyToResultAsDouble()
        {
            var jsonString = "{ \"integer-Property\": 123 }";

            const double expectedResult = 123;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["integer-Property"]);
            Assert.IsType<double>(result.Properties["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result.Properties["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithInt64PropertyOfMaxValue_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {Int64.MaxValue} }}";

            double expectedResult = Int64.MaxValue;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["integer-Property"]);
            Assert.IsType<double>(result.Properties["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result.Properties["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithInt64PropertyOfMinValue_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {Int64.MinValue} }}";

            double expectedResult = Int64.MinValue;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["integer-Property"]);
            Assert.IsType<double>(result.Properties["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result.Properties["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithDecimalProperty_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {123.23M} }}";

            double expectedResult = (double)123.23M;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["integer-Property"]);
            Assert.IsType<double>(result.Properties["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result.Properties["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithDoubleProperty_AddsPropertyToResultAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {double.MaxValue} }}";

            double expectedResult = double.MaxValue;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["integer-Property"]);
            Assert.IsType<double>(result.Properties["integer-Property"]);
            Assert.Equal(expected: expectedResult, actual: result.Properties["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithFloatProperty_AddsPropertyToResultRoundedAsDouble()
        {
            var jsonString = $"{{ \"integer-Property\": {999.999F} }}";

            const double expectedResult = 999.999F;

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["integer-Property"]);
            Assert.IsType<double>(result.Properties["integer-Property"]);
            Assert.Equal(expected: Math.Round(expectedResult,3), actual: result.Properties["integer-Property"]);
        }

        [Fact]
        public void Reading_ContentWithDateProperty_AddsPropertyToResult()
        {
            var thisDate = new DateTime(2020, 7, 9, 10, 51, 12);

            var jsonString = $"{{ \"date-Property\": \"2020-07-09T10:51:12\" }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["date-Property"]);
            Assert.IsType<DateTime>(result.Properties["date-Property"]);
            Assert.Equal(expected: thisDate, actual: (DateTime)result.Properties["date-Property"]);
        }

        [Fact]
        public void Reading_ContentWithGuidProperty_AddsPropertyToResult()
        {
            var thisGuid = Guid.Parse("1b935602-b202-4246-b48b-8e17291e5376");

            var jsonString = $"{{ \"guid-Property\": \"1b935602-b202-4246-b48b-8e17291e5376\" }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["guid-Property"]);
            Assert.IsType<Guid>(result.Properties["guid-Property"]);
            Assert.Equal(expected: thisGuid, actual: Guid.Parse(result.Properties["guid-Property"].ToString()));
        }

        [Fact]
        public void Reading_ContentWithBooleanTrueProperty_AddsPropertyToResult()
        {
            var thisBoolean = true;

            var jsonString = $"{{ \"boolean-Property\": true }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["boolean-Property"]);
            Assert.IsType<bool>(result.Properties["boolean-Property"]);
            Assert.Equal(expected: thisBoolean, actual: bool.Parse(result.Properties["boolean-Property"].ToString()));
        }

        [Fact]
        public void Reading_ContentWithBooleanFalseProperty_AddsPropertyToResult()
        {
            var thisBoolean = false;

            var jsonString = $"{{ \"boolean-Property\": false }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.NotNull(result.Properties["boolean-Property"]);
            Assert.IsType<bool>(result.Properties["boolean-Property"]);
            Assert.Equal(expected: thisBoolean, actual: bool.Parse(result.Properties["boolean-Property"].ToString()));
        }

        [Fact]
        public void Reading_ContentWithNullProperty_AddsPropertyToResult()
        {
            var jsonString = $"{{ \"some-Property\": null }}";

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.Null(result.Properties["some-Property"]);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, object>
            {
                { "property1", "hello" },
                { "property2", "world" }
            };

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.Equal(expected: expectedDictionary, actual: result.Properties["complex-Property"]);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

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

            Assert.NotNull(result.Properties);
            Assert.Single(result.Properties);
            Assert.Equal(expected: expectedDictionary, actual: result.Properties["complex-Property"]);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

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

            Assert.Equal(expected: "once", actual: result.Properties["story"].ToString());
            Assert.Equal(expected: "upon a time", actual: result.Properties["start"].ToString());
            Assert.Equal(expected: expectedDictionary, actual: result.Properties["complex-Property"]);
            Assert.Equal(expected: "the end", actual: result.Properties["end"].ToString());
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var expectedLinks = new Dictionary<string, IEnumerable<Link>>
            {
                { 
                    "self", 
                    new List<Link>
                    {
                        new Link("/api/categories", "Some Title")
                    }
                }
            };

            expectedLinks.ShouldDeepEqual(result.Links);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var expectedLinks = new Dictionary<string, IEnumerable<Link>>
            {
                { 
                    "self", new List<Link>
                    {
                        new Link("/api/categories/123", "Category")
                    }
                },
                {
                    "parent", new List<Link>
                    {
                        new Link("/api/categories", "Categories")
                    }
                }
            };

            expectedLinks.ShouldDeepEqual(result.Links);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var expectedLinks = new Dictionary<string, IEnumerable<Link>>
            {
                {
                    "orders", new List<Link>
                    {
                        new Link("/api/orders/123", "Order"),
                        new Link("/api/orders/456", "Order")
                    }
                }
            };

            expectedLinks.ShouldDeepEqual(result.Links);
        }

        [Fact]
        public void Reading_ContentWithLinksAndNoProperties_PopulatesLinksCollectionOnly()
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            Assert.NotNull(result.Links);
            Assert.NotNull(result.Properties);
            Assert.NotNull(result.EmbeddedItems);

            Assert.Single(result.Links);
            Assert.Empty(result.Properties);
            Assert.Empty(result.EmbeddedItems);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var exectedEmbeddedItems = new Dictionary<string, IEnumerable<HalReader>>
            {
                { 
                    "something",
                    new List<HalReader>
                    {
                        new HalReader
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            }
                        }
                    }
                }
            };

            Assert.NotNull(result.EmbeddedItems);
            Assert.Empty(result.Links);
            Assert.Empty(result.Properties);
            Assert.Equal(expected: 1, actual: result.EmbeddedItems.Count);
            Assert.Single(result.EmbeddedItems["something"]);
            exectedEmbeddedItems.ShouldDeepEqual(result.EmbeddedItems);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var exectedEmbeddedItems = new Dictionary<string, IEnumerable<HalReader>>
            {
                {
                    "something",
                    new List<HalReader>
                    {
                        new HalReader
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            }
                        }
                    }
                },
                {
                    "something-Else",
                    new List<HalReader>
                    {
                        new HalReader
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "bool-Property", true },
                                { "date-Property", new DateTime(2020, 7, 9, 10, 51, 12)}
                            }
                        }
                    }
                }
            };

            Assert.NotNull(result.EmbeddedItems);
            Assert.Equal(expected: 2, actual: result.EmbeddedItems.Count);
            Assert.Single(result.EmbeddedItems["something"]);
            Assert.Single(result.EmbeddedItems["something-Else"]);
            Assert.Empty(result.Links);
            Assert.Empty(result.Properties);
            exectedEmbeddedItems.ShouldDeepEqual(result.EmbeddedItems);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var exectedEmbeddedItems = new Dictionary<string, IEnumerable<HalReader>>
            {
                {
                    "something",
                    new List<HalReader>
                    {
                        new HalReader
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            }
                        },
                        new HalReader
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "string-Property", "Some different string" },
                                { "integer-Property", 321}
                            }
                        }
                    }
                }
            };

            Assert.NotNull(result.EmbeddedItems);
            Assert.Equal(expected: 1, actual: result.EmbeddedItems.Count);
            Assert.Equal(expected: 2, actual: result.EmbeddedItems["something"].Count());
            Assert.Empty(result.Links);
            Assert.Empty(result.Properties);
            exectedEmbeddedItems.ShouldDeepEqual(result.EmbeddedItems);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var exectedEmbeddedItems = new Dictionary<string, IEnumerable<HalReader>>
            {
                {
                    "something",
                    new List<HalReader>
                    {
                        new HalReader
                        {
                            Links = new Dictionary<string, IEnumerable<Link>>
                            {
                                { 
                                    "self", new List<Link> 
                                    {
                                        new Link("/api/categories", "Some Title")
                                    } 
                                }
                            },
                            Properties = new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            }
                        }
                    }
                }
            };

            Assert.NotNull(result.EmbeddedItems);
            Assert.Equal(expected: 1, actual: result.EmbeddedItems.Count);
            Assert.Single(result.EmbeddedItems["something"]);

            Assert.Empty(result.Links);
            Assert.Empty(result.Properties);

            exectedEmbeddedItems.ShouldDeepEqual(result.EmbeddedItems);
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, IEnumerable<HalReader>>
            {
                {
                    "something",
                    new List<HalReader>
                    {
                        new HalReader
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            },
                            EmbeddedItems = new Dictionary<string, IEnumerable<HalReader>>
                            {
                                { 
                                    "embedded-in-embedded-item",
                                    new List<HalReader>
                                    {
                                        new HalReader
                                        {
                                            Properties = new Dictionary<string, object>
                                            {
                                                { "bool-Property", true }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            expectedDictionary.ShouldDeepEqual(result.EmbeddedItems);
        }

        [Fact]
        public void Reading_ComplexExample_Works()
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
            serializeOptions.Converters.Add(new Deserializer());
            serializeOptions.WriteIndented = true;

            var result = JsonSerializer.Deserialize<HalReader>(jsonString, serializeOptions);

            var expectedDictionary = new Dictionary<string, IEnumerable<HalReader>>
            {
                {
                    "something",
                    new List<HalReader>
                    {
                        new HalReader
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "string-Property", "Some string" },
                                { "integer-Property", 123}
                            },
                            EmbeddedItems = new Dictionary<string, IEnumerable<HalReader>>
                            {
                                {
                                    "embedded-in-embedded-item",
                                    new List<HalReader>
                                    {
                                        new HalReader
                                        {
                                            Properties = new Dictionary<string, object>
                                            {
                                                { "bool-Property", true }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            expectedDictionary.ShouldDeepEqual(result.EmbeddedItems);
        }

    }
}