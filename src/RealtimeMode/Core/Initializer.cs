using AnN3x.ModdingLib;
using AnN3x.HumankindLib;

namespace AnN3x.RealtimeMode
{
    internal class Initializer
    {

        public static void Setup()
        {
            Loggr.Enabled = !Config.QuietMode;
            Loggr.WriteLogToDisk = Config.WriteLogToDisk;

            AnN3x.ModdingLib.Logging.PrintableValue.ValueParsers.Add(new HumankindPrintableValueParser());
            
            HumankindGame.Initialize();

            Loggr.Log("SETUP DONE", System.ConsoleColor.DarkCyan);
        }
        
        public static void Unload()
        {
            HumankindGame.Unload();
        }
    }
}
