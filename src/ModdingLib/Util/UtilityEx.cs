using System;

namespace AnN3x.ModdingLib;

public static class UtilityEx
{
    public static bool TryInvoke(this Action action, bool quiet = false)
    {
        bool success = true;
        
        if (action != null)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                if (!quiet)
                    Loggr.Log(e);
                success = false;
            }
        }

        return success;
    }
    
    public static bool TryInvoke(this Func<bool> action, bool quiet = false)
    {
        bool success = true;
        
        if (action != null)
        {
            try
            {
                success = action.Invoke();
            }
            catch (Exception e)
            {
                if (!quiet)
                    Loggr.Log(e);
                success = false;
            }
        }

        return success;
    }
}
