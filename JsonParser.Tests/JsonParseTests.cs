﻿using NUnit.Framework;

namespace JsonParser.Tests
{
    public class JsonParseTests
    {
        [Test, Ignore]
        public void TrueReturnsTrue()
        {
            var json = "true";

            var result = Json.Parse(json);

            Assert.That(result, Is.True);
        }
    }
}