#if !NOLOGGR
using System;
using static AnN3x.CoreLib.Logging.PrintableValue;

namespace AnN3x.CoreLib.Logging;

public class EnumValueParser : IPrintableValueParser
{
    public bool TryParse(object objectValue, Type objectType, out string result, out string fullType,
        out int lenMod)
    {
        fullType = UseFullTypeNames ? objectType.FullName : objectType.Name;
        lenMod = 0;

        if (objectType.IsEnum)
        {
            lenMod = PrintableValue.ColorType.EnumType.Length;
            result = PrintableValue.ColorType.EnumType + objectType.Name + "." + Enum.GetName(objectType, objectValue);
            fullType += " Enum";

        }
        else
        {
            result = null;
            return false;
        }

        return true;
    }
}

#endif
