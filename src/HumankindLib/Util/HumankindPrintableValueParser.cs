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
                case "UITransform":
                    var uiT = ((UITransform)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "UITransform @ " + ColorType.Default + uiT.Rect;
                    break;
                case "UITooltipData":
                    lenMod = ColorType.String.Length;
                    result = ColorType.String + $"\"{((Amplitude.UI.Interactables.UITooltipData)objectValue).Message}\"";
                    break;
                case "UITooltipClassDefinition":
                    lenMod = ColorType.String.Length;
                    result = ColorType.String + $"@\"{((Amplitude.UI.Tooltips.UITooltipClassDefinition)objectValue).Name.ToString()}\"";
                    break;
                case "RectMargins":
                    var r = ((RectMargins)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "RectMargins " + ColorType.Default + $"(l: {r.Left}, r: {r.Right}, t: {r.Top}, b: {r.Bottom})";
                    break;
                case "UIBorderAnchor":
                    var anchor = ((UIBorderAnchor)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "UIBorderAnchor " + ColorType.Default + $"(Attach: {anchor.Attach}, Percent: {anchor.Percent}, Margin: {anchor.Margin}, Offset: {anchor.Offset})";
                    break;
                case "UIPivotAnchor":
                    var pivot = ((UIPivotAnchor)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "UIPivotAnchor " + ColorType.Default + $"(Attach: {pivot.Attach}, Percent: {pivot.Percent}, MinMargin: {pivot.MinMargin}, MaxMargin: {pivot.MaxMargin}, Offset: {pivot.Offset})";
                    break;
                case "UIAtomId":
                    var atom = ((UIAtomId)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "UIAtomId " + ColorType.Default + $"(Index: {atom.Index}, Allocator: {atom.Allocator}, IsValid: {atom.IsValid})";
                    break;
                case "UIStamp":
                    var stamp = ((UIStamp)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "UIStamp " + ColorType.Default + $"(RegistrationId: {stamp.RegistrationId}, KeyGuid: {stamp.KeyGuid}, IsLoaded: {stamp.IsLoaded})";
                    break;
                case "AffineTransform2d":
                    var at = ((AffineTransform2d)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length;
                    result = ColorType.HeadingType + "AffineTransform2d " + ColorType.Default + $"Translation: {at.Translation.ToString()}, Rotation: {at.Rotation.ToString()}, Scale: {at.Scale.ToString()}";
                    break;
                case "UIMaterialId":
                    lenMod = ColorType.HeadingType.Length + ColorType.String.Length;
                    result = ColorType.HeadingType + "UIMaterialId " + ColorType.String + $"@\"{((UIMaterialId)objectValue).Id.ToString()}\"";
                    break;
                case "UITexture":
                    var uiTex = ((UITexture)objectValue);
                    lenMod = ColorType.HeadingType.Length + ColorType.Default.Length + ColorType.AdditionalInfo.Length;
                    result = ColorType.HeadingType + "UITexture" + ColorType.Default + " " + uiTex.AssetPath
                                + " " + ColorType.AdditionalInfo + uiTex.WidthHeight.x + "x" + uiTex.WidthHeight.y + "px";
                    break;
                default:
                    if (objectValue is MonoBehaviour)
                    {
                        lenMod = ColorType.EnumType.Length + ColorType.NotFound.Length;
                        result = ColorType.EnumType + "<MonoBehaviour> " + ColorType.NotFound + objectType.Name;
                    }
                    else
                    {
                        result = null;
                        return false;
                    }

                    break;
            }

            return true;
        }
    }
}
