using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AnN3x.CoreLib.Reflection;

public class TypeAccess
{
    public static BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static |
                                        BindingFlags.NonPublic;
    
    /// <summary>
    /// Gets the first Type found by name, searching only those
    /// assemblies matching any of the given filters, if any.  
    /// </summary>
    /// <returns></returns>
    public static Type GetTypeByName(string fullName, params string[] filters)
    {
        var assemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(ass =>
                filters.Length == 0 || filters.Any(filter =>
                    ass.FullName.Contains(filter))).ToList();

        foreach (var asm in assemblies)
        {
            IEnumerable<Type> types;
            
            try
            {
                types = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(x => x != null);
            }

            var match = types.FirstOrDefault(
                type => /* TODO: !!!CONTAINS */ type.FullName.Contains(fullName));

            if (match == null)
            {
                Loggr.Log("NO MATCH FOUND in Assembly: " + asm.FullName);
                continue;
            }
            Loggr.Log("!!! MATCH FOUND in Assembly: " + asm.FullName);
            Loggr.Log("\t" + match.FullName);
            Loggr.Log("\t" + match.Name);
        }

        return default; // TODO
    }
}
