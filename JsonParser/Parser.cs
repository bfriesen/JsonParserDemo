using System;
using Sprache;

namespace JsonParser
{
    internal static class Parser
    {
        public static Func<string, object> GetJsonParser()
        {
            var literalParser = GetLiteralParser();

            var mainParser = literalParser;

            return mainParser.Parse;
        }

        private static Parser<object> GetLiteralParser()
        {
            var trueParser =
                from t in Parse.String("true")
                select (object)true;

            var falseParser =
                from f in Parse.String("false")
                select (object)false;

            var nullParser =
                from f in Parse.String("null")
                select (object)null;

            var literalParser = trueParser.Or(falseParser).Or(nullParser);

            return literalParser;
        }
    }
}