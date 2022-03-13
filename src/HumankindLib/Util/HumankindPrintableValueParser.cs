using Amplitude.Framework.Simulation;
using Amplitude.Mercury;
using Amplitude.UI;
using Amplitude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static AnN3x.ModdingLib.Logging.PrintableValue;
using UnityEngine;
using AnN3x.ModdingLib.Logging;

namespace AnN3x.HumankindLib
{
    internal class HumankindPrintableValueParser : IPrintableValueParser
    {
        public bool TryParse(object objectValue, Type objectType, out string result, out string fullType, out int lenMod)
        {
            fullType = UseFullTypeNames ? objectType.FullName : objectType.Name;
            lenMod = 0;

            switch (objectType.Name)
            {
                // HUMANKIND TYPES
                case "FixedPoint":
                    result = ((int)((FixedPoint)objectValue)).ToString();
                    fullType = "FixedPoint";
                    break;
                case "Territory[]":
                    var territories = (Amplitude.Mercury.Interop.AI.Entities.Territory[])objectValue;
                    var territoryIndices = territories.Aggregate("",
                        (total, next) => total += (total.Length > 0 ? ", " : "") + next.TerritoryIndex);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "Territory[" + territories.Length + "]" + ColorType.Default + " { " + territoryIndices + " }";
                    break;
                case "WorldPosition":
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "WorldPosition " + ColorType.Default + ((WorldPosition)objectValue) + ", TileIndex " + ((WorldPosition)objectValue).ToTileIndex();
                    break;
                case "Property":
                    result = ((int)((Property)objectValue).Value).ToString();
                    fullType = "Property";
                    break;
                case "EditableProperty":
                    result = ((int)((EditableProperty)objectValue).Value).ToString();
                    fullType = "EditableProperty";
                    break;
                case "EntityNameInfo":
                    result = ((Amplitude.Mercury.Interop.EntityNameInfo)objectValue).ToString();
                    fullType = "EntityNameInfo";
                    break;
                case "StaticString":
                    result = "@" + ColorType.String + "\"" + ((Amplitude.StaticString)objectValue).ToString() + "\"";
                    lenMod = ColorType.String.Length;
                    fullType = "StaticString";
                    break;

                case "ArmyActionFailureFlags":
                    result = ((Amplitude.Mercury.Interop.ArmyActionFailureFlags)objectValue).ToString();
                    fullType += " Struct";
                    break;

                case "IndexRange":
                    result = ColorType.HeadingType + "IndexRange " + ColorType.Default + ((IndexRange)objectValue).ToString();
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "IndexRange";
                    break;
                default:
                    result = null;
                    return false;
            }

            return true;
        }
    }
}
