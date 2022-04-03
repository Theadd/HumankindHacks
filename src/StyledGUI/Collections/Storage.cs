using System.Collections.Generic;

namespace AnN3x.StyledGUI.Collections;

public static class Storage
{
    public static List<IStorageService> StorageServices = new List<IStorageService>();

    public static T Get<T>(StringHandle name) where T : IStoredType
    {
        foreach (var service in StorageServices)
            if (service.TryGet(name, out T value))
                return value;

        return default;
    }
}
