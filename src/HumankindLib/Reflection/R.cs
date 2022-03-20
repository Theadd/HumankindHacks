using System.Reflection;
using System;

namespace AnN3x.HumankindLib.Reflection;

public partial class R
{
    public const BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;
    public const BindingFlags PublicStatic = BindingFlags.Public | BindingFlags.Static;
    public const BindingFlags NonPublicStatic = BindingFlags.NonPublic | BindingFlags.Static;
    public const BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
    public const BindingFlags Flags = PublicInstance | NonPublicStatic;

    public static FieldInfo GetField<T>(string fieldName, BindingFlags bindingFlags = Flags) =>
        typeof(T).GetField(fieldName, bindingFlags);

    /// <summary>
    /// Searches for the specified method whose parameters match the specified argument types.
    /// </summary>
    /// <param name="methodName">A string containing the name of the method to get.</param>
    /// <param name="bindingFlags">A bitmask comprised of one or more BindingFlags.</param>
    /// <param name="parameterTypes">
    ///     (Optional) An array of method's parameter types, i.e.: `new Type[] { typeof(string), typeof(int) };`
    ///     for a method with two parameters, string and int.
    /// </param>
    /// <typeparam name="T">The Type that contains the method.</typeparam>
    /// <returns>
    ///     A MethodInfo object representing the method that matches the specified requirements, if found. Otherwise,
    ///     **null**.
    /// </returns>
    public static MethodInfo GetMethod<T>(string methodName, BindingFlags bindingFlags,
        Type[] parameterTypes = null)
    {
        return typeof(T).GetMethod(methodName, bindingFlags, null, parameterTypes ?? new Type[] { }, null);
    }

    /// <summary>
    /// Searches for the specified method by name
    /// </summary>
    /// <param name="methodName">Target method's name.</param>
    /// <typeparam name="T">The type to search the method in.</typeparam>
    /// <returns><c>MethodInfo</c></returns>
    public static MethodInfo GetMethod<T>(string methodName) => typeof(T).GetMethod(methodName, Flags);
    
    public static Object GetPropValue(Object obj, String name) {
        foreach (String part in name.Split('.')) {
            if (obj == null) { return null; }

            Type type = obj.GetType();
            PropertyInfo info = type.GetProperty(part, R.Flags);
            if (info == null) { return null; }

            obj = info.GetValue(obj, null);
        }
        return obj;
    }

    public static T GetPropValue<T>(Object obj, String name) {
        Object retval = GetPropValue(obj, name);
        if (retval == null) { return default(T); }

        // throws InvalidCastException if types are incompatible
        return (T) retval;
    }
}
