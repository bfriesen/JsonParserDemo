using System;

namespace JsonParser
{
    public class Json
    {
        private static readonly Func<string, dynamic> _parse = Parser.GetJsonParser();

        public static dynamic Parse(string json)
        {
            return _parse(json);
        }
    }
}