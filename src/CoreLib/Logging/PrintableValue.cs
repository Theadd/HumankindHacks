#if !NOLOGGR
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnN3x.CoreLib.Logging
{
    internal static class PrintableValue
    {
        public static class ColorType
        {
            public static string NotFound = "%Red%";
            public static string HeadingType = "%DarkGreen%";
            public static string EnumType = "%Green%";
            public static string FullType = "%DarkGray%";
            public static string NotImportant = "%DarkGray%";
            public static string Error = "%DarkRed%";
            public static string Default = "%White%";
            public static string String = "%DarkYellow%";
            public static string AdditionalInfo = "%DarkYellow%";
        }

        public static List<IPrintableValueParser> ValueParsers = new List<IPrintableValueParser>()
        {
            new EnumValueParser(),
            new CommonValueParser()
        };

        public static bool UseFullTypeNames { get; set; } = false;

        public static string MergeValueAndType(string valueString, string typeString, int lenMod)
        {
            var padRight = lenMod > 0 ? new string(' ', lenMod) : "";
            var realLength = valueString.Length - lenMod;
            if (realLength >= 83)
            {
                valueString = valueString.Substring(0, 80 + lenMod) + "...";
            }
            return $"{valueString,-84}" + padRight + ColorType.FullType + "// " + typeString;
            // return valueString + "\t\t" + ColorType.FullType + "// " + typeString;
        }

        public static string KeepValueOnly(string valueString, string typeString, int lenMod)
        {
            var padRight = lenMod > 0 ? new string(' ', lenMod) : "";
            var realLength = valueString.Length - lenMod;
            if (realLength >= 123)
            {
                valueString = valueString.Substring(0, 120 + lenMod) + "...";
            }
            return $"{valueString,-124}" + padRight;    // + ColorType.FullType + "// " + typeString;
            // return valueString + "\t\t" + ColorType.FullType + "// " + typeString;
        }

        public static string AsValueOnlyString(object objectValue, Type objectType)
        {
            string result = string.Empty;
            string fullType = string.Empty;
            int lenMod = 0;

            if (objectValue == null)
            {
                return KeepValueOnly(
                    ColorType.FullType + "null",
                    objectType.FullName,
                    ColorType.FullType.Length
                );
            }
            
            if (ValueParsers.Any(parser => parser.TryParse(objectValue, objectType, out result, out fullType, out lenMod)))
                return KeepValueOnly(result, fullType, lenMod);

            return ColorType.NotFound + objectType.Name;
        }

        public static string AsString(object objectValue, Type objectType)
        {
            string result = string.Empty;
            string fullType = string.Empty;
            int lenMod = 0;

            if (objectValue == null)
            {
                return MergeValueAndType(
                    ColorType.FullType + "null",
                    objectType.FullName,
                    ColorType.FullType.Length
                );
            }

            if (ValueParsers.Any(parser => parser.TryParse(objectValue, objectType, out result, out fullType, out lenMod)))
                return MergeValueAndType(result, fullType, lenMod);

            return ColorType.NotFound + objectType.Name;
        }
    }
}

#endif
