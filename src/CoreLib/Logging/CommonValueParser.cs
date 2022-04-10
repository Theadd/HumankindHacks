#if !NOLOGGR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Amplitude.Mercury.Interop.AI.Data;
using BepInEx.Configuration;
using static AnN3x.CoreLib.Logging.PrintableValue;
using UnityEngine;

namespace AnN3x.CoreLib.Logging
{
    internal class CommonValueParser : IPrintableValueParser
    {
        public bool TryParse(object objectValue, Type objectType, out string result, out string fullType, out int lenMod)
        {
            fullType = UseFullTypeNames ? objectType.FullName : objectType.Name;
            lenMod = 0;

            switch (objectType.Name)
            {
                // COMMON TYPES
                case "Int32":
                    result = ((int)objectValue).ToString();
                    fullType = "int";
                    break;
                case "Boolean":
                    result = ((Boolean)objectValue).ToString();
                    fullType = "bool";
                    break;
                case "String":
                    result = PrintableValue.ColorType.String + "\"" + ((string)objectValue) + "\"";
                    lenMod = PrintableValue.ColorType.String.Length;
                    fullType = "string";
                    break;
                case "Single":
                    result = ((float)objectValue).ToString();
                    fullType = "float";
                    break;
                case "UInt64":
                    result = ((UInt64)objectValue).ToString();
                    fullType = "ulong";
                    break;
                case "Int64":
                    result = ((Int64)objectValue).ToString();
                    fullType = "long";
                    break;
                case "List`1":
                    var listLength = ((ICollection)objectValue)?.Count;
                    result = PrintableValue.ColorType.NotFound + "List<" + objectType.GetGenericArguments().FirstOrDefault().Name + ">[" + listLength + "]";
                    fullType = "List<" + objectType.GetGenericArguments().FirstOrDefault().FullName + ">";
                    lenMod = PrintableValue.ColorType.NotFound.Length;
                    break;
                case "Unit[]":
                    var units = (Unit[])objectValue;
                    var unitNames = string.Join(", ", units.AsEnumerable().Select(unit => unit.UnitDefinition.Name.ToString().Split('_').LastOrDefault()).ToArray());
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    result = PrintableValue.ColorType.HeadingType + "Unit[" + units.Length + "]" + PrintableValue.ColorType.Default + " { " + unitNames + " }";
                    break;
                case "Vector2[]":
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length + PrintableValue.ColorType.HeadingType.Length;
                    result = PrintableValue.ColorType.HeadingType + "Vector2[" + PrintableValue.ColorType.Default + ((Vector2[])objectValue).Length + PrintableValue.ColorType.HeadingType + "]";
                    fullType = "Vector2[]";
                    break;
                case "UInt16[]":
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length + PrintableValue.ColorType.HeadingType.Length;
                    result = PrintableValue.ColorType.HeadingType + "UInt16[" + PrintableValue.ColorType.Default + ((UInt16[])objectValue).Length + PrintableValue.ColorType.HeadingType + "]";
                    fullType = "UInt16[]";
                    break;
                case "UInt64[]":
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length + PrintableValue.ColorType.HeadingType.Length;
                    result = PrintableValue.ColorType.HeadingType + "UInt64[" + PrintableValue.ColorType.Default + ((UInt64[])objectValue).Length + PrintableValue.ColorType.HeadingType + "]";
                    fullType = "UInt64[]";
                    break;
                case "Int64[]":
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length + PrintableValue.ColorType.HeadingType.Length;
                    result = PrintableValue.ColorType.HeadingType + "Int64[" + PrintableValue.ColorType.Default + ((Int64[])objectValue).Length + PrintableValue.ColorType.HeadingType + "]";
                    fullType = "Int64[]";
                    break;
                case "RectOffset":
                    result = ((RectOffset)objectValue).ToString();
                    fullType = "RectOffset";
                    break;
                case "Rect":
                    result = ((Rect)objectValue).ToString();
                    fullType = "Rect";
                    break;
                case "Bounds":
                    result = ((Bounds)objectValue).ToString();
                    fullType = "Bounds";
                    break;
                case "Vector2":
                    result = ((Vector2)objectValue).ToString();
                    fullType = "Vector2";
                    break;
                case "Vector3":
                    result = ((Vector3)objectValue).ToString();
                    fullType = "Vector3";
                    break;
                case "Vector4":
                    result = ((Vector4)objectValue).ToString();
                    fullType = "Vector4";
                    break;
                case "Texture":
                    result = PrintableValue.ColorType.HeadingType + "Texture " + PrintableValue.ColorType.Default + ((Texture)objectValue).name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Texture";
                    break;
                case "Texture2D":
                    result = PrintableValue.ColorType.HeadingType + "Texture2D " + PrintableValue.ColorType.Default + ((Texture2D)objectValue).name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Texture2D";
                    break;
                case "Sprite":
                    result = PrintableValue.ColorType.HeadingType + "Sprite " + PrintableValue.ColorType.Default + ((Sprite)objectValue).name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Sprite";
                    break;
                case "Type":
                    result = PrintableValue.ColorType.HeadingType + "Type " + PrintableValue.ColorType.Default + ((Type)objectValue).Name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Type";
                    break;
                case "Assembly":
                    result = PrintableValue.ColorType.HeadingType + "Assembly " + PrintableValue.ColorType.Default + ((Assembly)objectValue).GetName();
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Assembly";
                    break;
                case "Module":
                    result = PrintableValue.ColorType.HeadingType + "Module " + PrintableValue.ColorType.Default + ((Module)objectValue).Name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Module";
                    break;
                case "KeyboardShortcut":
                    result = PrintableValue.ColorType.HeadingType + "KeyboardShortcut " + PrintableValue.ColorType.Default + ((KeyboardShortcut)objectValue).ToString();
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    break;
                case "MethodInfo":
                    result = PrintableValue.ColorType.HeadingType + "MethodInfo " + PrintableValue.ColorType.Default + ((MethodInfo)objectValue).Name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "MethodInfo";
                    break;
                case "FieldInfo":
                    result = PrintableValue.ColorType.HeadingType + "FieldInfo " + PrintableValue.ColorType.Default + ((FieldInfo)objectValue).Name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "FieldInfo";
                    break;
                case "PropertyInfo":
                    result = PrintableValue.ColorType.HeadingType + "PropertyInfo " + PrintableValue.ColorType.Default + ((PropertyInfo)objectValue).Name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "PropertyInfo";
                    break;
                case "Color":
                    result = PrintableValue.ColorType.HeadingType + "Color " + PrintableValue.ColorType.Default + ((Color)objectValue).ToString();
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Color";
                    break;
                case "Color32":
                    result = PrintableValue.ColorType.HeadingType + "Color32 " + PrintableValue.ColorType.Default + ((Color32)objectValue).ToString();
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Color32";
                    break;
                case "GUIStyleState":
                    result = PrintableValue.ColorType.HeadingType + "GUIStyleState " + PrintableValue.ColorType.Default + ((GUIStyleState)objectValue).background.name + ", " + ((GUIStyleState)objectValue).textColor.ToString();
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "GUIStyleState";
                    break;
                case "Font":
                    result = PrintableValue.ColorType.HeadingType + "Font " + PrintableValue.ColorType.Default + ((Font)objectValue).name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Font";
                    break;
                case "GUIStyle":
                    result = PrintableValue.ColorType.HeadingType + "GUIStyle " + PrintableValue.ColorType.Default + ((GUIStyle)objectValue).name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "GUIStyle";
                    break;
                case "Material":
                    result = PrintableValue.ColorType.HeadingType + "Material " + PrintableValue.ColorType.Default + ((Material)objectValue).name;
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length;
                    fullType = "Material";
                    break;
                case "Transform":
                    var t = ((Transform)objectValue);
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.Default.Length + PrintableValue.ColorType.AdditionalInfo.Length;
                    result = PrintableValue.ColorType.HeadingType + "Transform @ " + PrintableValue.ColorType.Default + t.position +
                             (t.childCount > 0 ? PrintableValue.ColorType.AdditionalInfo + " [+" + t.childCount + "]" : "");
                    break;
                case "GameObject":
                    lenMod = PrintableValue.ColorType.HeadingType.Length + PrintableValue.ColorType.String.Length;
                    result = PrintableValue.ColorType.HeadingType + "GameObject " + PrintableValue.ColorType.String + "\"" + ((GameObject)objectValue).name + "\"";
                    break;
                default:
                    result = null;
                    return false;
            }

            return true;
        }
    }
}

#endif
