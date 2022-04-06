using System;
using AnN3x.ModdingLib;
using AnN3x.HumankindLib;
using UnityEngine;

namespace AnN3x.EndlessMovingArmies
{
    internal class Initializer
    {
        public static bool IsReadyToInitialize() => GameObject.Find("/WindowsRoot/SystemOverlays") != null;

        public static void Setup()
        {
            Loggr.Enabled = !Config.QuietMode;
            Loggr.WriteLogToDisk = Config.WriteLogToDisk;
#if !NOLOGGR
            ModdingLib.Logging.PrintableValue.ValueParsers.Add(new HumankindPrintableValueParser());
#endif
        }

        public static bool Initialize()
        {
            bool success = false;
    
            if (!IsReadyToInitialize())
                return false;

            try
            {
                HumankindGame.Initialize();
                success = true;
            }
            catch (Exception e)
            {
                Loggr.Log(e);
            }

            return success;
        }

        public static void Unload()
        {
            HumankindGame.Unload();
        }
    }
}
