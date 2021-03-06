#if !BEPINEX
#define NOLOGGR
#endif
#if !NOLOGGR
using System;
using System.Collections.Generic;
using System.Text;

namespace AnN3x.CoreLib.Logging
{
    public interface IPrintableValueParser
    {

        public bool TryParse(object objectValue, Type objectType, out string result, out string fullType, out int lenMod);

    }
}

#endif
