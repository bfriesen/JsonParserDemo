using System;

namespace JsonParser
{
    public static class Json
    {
        private static readonly Func<string, dynamic> _parse = Parser.GetJsonParser();

        public static dynamic Parse(string json)
        {
            return _parse(json);
        }
    }
}