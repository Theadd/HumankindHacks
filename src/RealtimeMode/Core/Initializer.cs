using AnN3x.ModdingLib;
using AnN3x.HumankindLib;

namespace AnN3x.RealtimeMode
{
    internal class Initializer
    {

        public static void Setup()
        {
            Loggr.Enabled = !FeatureFlags.QuietMode;
            Loggr.WriteLogToDisk = FeatureFlags.WriteLogToDisk;

            AnN3x.ModdingLib.Logging.PrintableValue.ValueParsers.Add(new HumankindPrintableValueParser());

            Loggr.Log("SETUP DONE", System.ConsoleColor.DarkCyan);
        }
    }
}
