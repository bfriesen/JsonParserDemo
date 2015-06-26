using System;
using System.Collections.Generic;
using System.Dynamic;
using Sprache;

namespace JsonParser
{
    internal static class Parser
    {
        public static Func<string, object> GetJsonParser()
        {
            var mainParser = new MainParser();

            var literalParser = GetLiteralParser();
            var stringParser = GetStringParser();
            var objectParser = GetObjectParser(stringParser, mainParser);
            var arrayParser = GetArrayParser(mainParser);

            mainParser.Value = literalParser.Or(stringParser).Or(objectParser).Or(arrayParser);

            return mainParser.Value.Parse;
        }

        private static Parser<object> GetArrayParser(MainParser mainParser)
        {
            return 
                from openSquare in Parse.Char('[')
                from closeSquare in Parse.Char(']')
                select new object[0];
        }

        private class MainParser
        {
            public Parser<object> Value;
        }

        private static Parser<object> GetObjectParser(
            Parser<string> stringParser, MainParser mainParser)
        {
            var memberParser =
                from name in stringParser
                from colon in Parse.Char(':')
                from value in Parse.Ref(() => mainParser.Value)
                select new Member { Name = name, Value = value };

            var objectParser =
                from openCurly in Parse.Char('{')
                from first in memberParser.Optional()
                from rest in
                    (from comma in Parse.Char(',')
                     from member in memberParser
                     select member).Many()
                from closeCurly in Parse.Char('}')
                select GetExpandoObject(first.GetOrDefault(), rest);

            return objectParser;
        }

        private static ExpandoObject GetExpandoObject(
            Member first,
            IEnumerable<Member> rest)
        {
            var expandoObject = new ExpandoObject();

            var d = (IDictionary<string, object>)expandoObject;

            if (first != null)
            {
                d.Add(first.Name, first.Value);
            }

            foreach (var member in rest)
            {
                d.Add(member.Name, member.Value);
            }

            return expandoObject;
        }

        private class Member
        {
            public string Name;
            public object Value;
        }

        private static Parser<string> GetStringParser()
        {
            var escapedQuoteParser =
                from backslash in Parse.Char('\\')
                from quote in Parse.Char('"')
                select quote;

            var escapedBackslashParser =
                from backslash in Parse.Char('\\')
                from secondBackslash in Parse.Char('\\')
                select backslash;

            return
                from openQuote in Parse.Char('"')
                from value in escapedBackslashParser.Or(escapedQuoteParser).Or(Parse.CharExcept('"')).Many().Text()
                from closeQuote in Parse.Char('"')
                select value;
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