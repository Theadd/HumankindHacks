#if BEPINEX
using System;
using System.Reflection;
using AnN3x.CoreLib.Reflection;
// using HumankindModTool.Core;

namespace AnN3x.CoreLib;

public static class UnityExplorer
{
    private static bool isFirstCallToInspect = true;
    private static Type inspectorManagerType;
    private static Type cacheObjectBaseType;

    /// <summary>
    /// Tells whether UnityExplorer plugin is currently loaded or not.
    /// </summary>
    /// <returns></returns>
    public static bool CanInspect() => !isFirstCallToInspect
        ? inspectorManagerType != null
        : (isFirstCallToInspect = false) !=
          ((inspectorManagerType = TypeAccess.GetTypeByName("UnityExplorer.InspectorManager", "UnityExplorer")) !=
           null);

    /// <summary>
    /// Sends a type to open in a new tab of UnityExplorer's Inspector, if available.
    /// </summary>
    /// <param name="type">a Type to inspect</param>
    public static void Inspect(Type type)
    {
        if (!CanInspect())
            return;

        inspectorManagerType.GetMethod("Inspect", TypeAccess.FLAGS, null,
            new Type[] { typeof(Type) }, null)?.Invoke(null, new object[] { type });
    }

    /// <summary>
    /// Sends an object to UnityExplorer's Inspector, if available.
    /// </summary>
    /// <param name="obj">an object to inspect</param>
    public static void Inspect(object obj, object sourceCache = null)
    {
        if (!CanInspect())
            return;

        cacheObjectBaseType ??= TypeAccess.GetTypeByName("UnityExplorer.CacheObject.CacheObjectBase", "UnityExplorer");

        inspectorManagerType.GetMethod("Inspect", TypeAccess.FLAGS, null,
                new Type[] { typeof(object), cacheObjectBaseType }, null)
            ?.Invoke(null, new object[] { obj, sourceCache });
    }
}
#endif
