using System;

namespace JsonParser
{
    public static class Json
    {
        private static readonly Func<string, object> _parse = Parser.GetJsonParser();

        public static dynamic Parse(string json)
        {
            return _parse(json);
        }
    }
}