using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;

namespace JsonParser.Tests
{
    public class JsonParseTests
    {
        [Test]
        public void TrueReturnsTrue()
        {
            var json = "true";

            var result = Json.Parse(json);

            Assert.That(result, Is.True);
        }

        [Test]
        public void FalseReturnsFalse()
        {
            var json = "false";

            var result = Json.Parse(json);

            Assert.That(result, Is.False);
        }

        [Test]
        public void NullReturnsNull()
        {
            var json = "null";

            var result = Json.Parse(json);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void StringWithoutEscapeSequencesReturnsString()
        {
            var json = "\"Hello, world!\"";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo("Hello, world!"));
        }

        [Test]
        public void StringWithEscapedQuoteSequencesReturnsString()
        {
            var json = "\"\\\"Ow.\\\" - my pancreas\"";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo("\"Ow.\" - my pancreas"));
        }

        [Test]
        public void StringWithEscapedBackslashSequencesReturnsString()
        {
            var json = "\"c:\\\\temp\\\\file.txt\"";

            var result = Json.Parse(json);

            Assert.That(result, Is.EqualTo("c:\\temp\\file.txt"));
        }

        [Test]
        public void EmptyObjectReturnsExpandoObject()
        {
            var json = "{}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());
            var d = (IDictionary<string, object>)result;
            Assert.That(d.Count, Is.EqualTo(0));
        }

        [Test]
        public void ObjectWithSinglePrimitiveMemberReturnsExpandoObject()
        {
            var json = "{\"foo\":true}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());
            var d = (IDictionary<string, object>)result;
            Assert.That(d.Count, Is.EqualTo(1));
            Assert.That(result.foo, Is.True);
        }

        [Test]
        public void ObjectWithMultiplePrimitiveMemberReturnsExpandoObject()
        {
            var json = "{\"foo\":true,\"bar\":false}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());
            var d = (IDictionary<string, object>)result;
            Assert.That(d.Count, Is.EqualTo(2));
            Assert.That(result.foo, Is.True);
            Assert.That(result.bar, Is.False);
        }

        [Test]
        public void ObjectWithObjectMembersReturnsExpandoObject()
        {
            var json = "{\"foo\":{\"qux\":null},\"bar\":{\"corge\":false}}";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<ExpandoObject>());
            var d = (IDictionary<string, object>)result;
            Assert.That(d.Count, Is.EqualTo(2));
            Assert.That(result.foo.qux, Is.Null);
            Assert.That(result.bar.corge, Is.False);
        }

        [Test]
        public void EmptyArrayReturnsObjectArray()
        {
            var json = "[]";

            var result = Json.Parse(json);

            Assert.That(result, Is.InstanceOf<object[]>());
            Assert.That(result, Is.Empty);
        }
    }
}