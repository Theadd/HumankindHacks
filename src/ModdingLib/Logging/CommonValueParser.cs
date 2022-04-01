#if !NOLOGGR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Amplitude.Mercury.Interop.AI.Data;
using BepInEx.Configuration;
using static AnN3x.ModdingLib.Logging.PrintableValue;
using UnityEngine;

namespace AnN3x.ModdingLib.Logging
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
                    result = ColorType.String + "\"" + ((string)objectValue) + "\"";
                    lenMod = ColorType.String.Length;
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
                    result = ColorType.NotFound + "List<" + objectType.GetGenericArguments().FirstOrDefault().Name + ">[" + listLength + "]";
                    fullType = "List<" + objectType.GetGenericArguments().FirstOrDefault().FullName + ">";
                    lenMod = ColorType.NotFound.Length;
                    break;
                case "Unit[]":
                    var units = (Unit[])objectValue;
                    var unitNames = string.Join(", ", units.AsEnumerable().Select(unit => unit.UnitDefinition.Name.ToString().Split('_').LastOrDefault()).ToArray());
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "Unit[" + units.Length + "]" + ColorType.Default + " { " + unitNames + " }";
                    break;
                case "Vector2[]":
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length + ColorType.HeadingType.Length;
                    result = ColorType.HeadingType + "Vector2[" + ColorType.Default + ((Vector2[])objectValue).Length + ColorType.HeadingType + "]";
                    fullType = "Vector2[]";
                    break;
                case "UInt16[]":
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length + ColorType.HeadingType.Length;
                    result = ColorType.HeadingType + "UInt16[" + ColorType.Default + ((UInt16[])objectValue).Length + ColorType.HeadingType + "]";
                    fullType = "UInt16[]";
                    break;
                case "UInt64[]":
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length + ColorType.HeadingType.Length;
                    result = ColorType.HeadingType + "UInt64[" + ColorType.Default + ((UInt64[])objectValue).Length + ColorType.HeadingType + "]";
                    fullType = "UInt64[]";
                    break;
                case "Int64[]":
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length + ColorType.HeadingType.Length;
                    result = ColorType.HeadingType + "Int64[" + ColorType.Default + ((Int64[])objectValue).Length + ColorType.HeadingType + "]";
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
                    result = ColorType.HeadingType + "Texture " + ColorType.Default + ((Texture)objectValue).name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Texture";
                    break;
                case "Texture2D":
                    result = ColorType.HeadingType + "Texture2D " + ColorType.Default + ((Texture2D)objectValue).name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Texture2D";
                    break;
                case "Sprite":
                    result = ColorType.HeadingType + "Sprite " + ColorType.Default + ((Sprite)objectValue).name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Sprite";
                    break;
                case "Type":
                    result = ColorType.HeadingType + "Type " + ColorType.Default + ((Type)objectValue).Name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Type";
                    break;
                case "Assembly":
                    result = ColorType.HeadingType + "Assembly " + ColorType.Default + ((Assembly)objectValue).GetName();
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Assembly";
                    break;
                case "Module":
                    result = ColorType.HeadingType + "Module " + ColorType.Default + ((Module)objectValue).Name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Module";
                    break;
                case "KeyboardShortcut":
                    result = ColorType.HeadingType + "KeyboardShortcut " + ColorType.Default + ((KeyboardShortcut)objectValue).ToString();
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    break;
                case "MethodInfo":
                    result = ColorType.HeadingType + "MethodInfo " + ColorType.Default + ((MethodInfo)objectValue).Name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "MethodInfo";
                    break;
                case "FieldInfo":
                    result = ColorType.HeadingType + "FieldInfo " + ColorType.Default + ((FieldInfo)objectValue).Name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "FieldInfo";
                    break;
                case "PropertyInfo":
                    result = ColorType.HeadingType + "PropertyInfo " + ColorType.Default + ((PropertyInfo)objectValue).Name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "PropertyInfo";
                    break;
                case "Color":
                    result = ColorType.HeadingType + "Color " + ColorType.Default + ((Color)objectValue).ToString();
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Color";
                    break;
                case "Color32":
                    result = ColorType.HeadingType + "Color32 " + ColorType.Default + ((Color32)objectValue).ToString();
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Color32";
                    break;
                case "GUIStyleState":
                    result = ColorType.HeadingType + "GUIStyleState " + ColorType.Default + ((GUIStyleState)objectValue).background.name + ", " + ((GUIStyleState)objectValue).textColor.ToString();
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "GUIStyleState";
                    break;
                case "Font":
                    result = ColorType.HeadingType + "Font " + ColorType.Default + ((Font)objectValue).name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Font";
                    break;
                case "GUIStyle":
                    result = ColorType.HeadingType + "GUIStyle " + ColorType.Default + ((GUIStyle)objectValue).name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "GUIStyle";
                    break;
                case "Material":
                    result = ColorType.HeadingType + "Material " + ColorType.Default + ((Material)objectValue).name;
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    fullType = "Material";
                    break;
                case "Transform":
                    var t = ((Transform)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length + ColorType.AdditionalInfo.Length;
                    result = ColorType.HeadingType + "Transform @ " + ColorType.Default + t.position +
                             (t.childCount > 0 ? ColorType.AdditionalInfo + " [+" + t.childCount + "]" : "");
                    break;
                case "GameObject":
                    lenMod = ColorType.HeadingType.Length + ColorType.String.Length;
                    result = ColorType.HeadingType + "GameObject " + ColorType.String + "\"" + ((GameObject)objectValue).name + "\"";
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
