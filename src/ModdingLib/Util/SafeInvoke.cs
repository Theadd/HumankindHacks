using System;

namespace AnN3x.ModdingLib;

public static class SafeInvoke
{
    public static void All(params Action[] actions) => All(true, actions);

    public static void All(bool quiet, params Action[] actions)
    {
        foreach (var action in actions) action.TryInvoke(quiet);
    }

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
