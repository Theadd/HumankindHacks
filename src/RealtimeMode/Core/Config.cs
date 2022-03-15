namespace AnN3x.RealtimeMode
{
    internal class FeatureFlags
    {
        /// <summary>
        /// Disables AnN3x.ModdingLib.Loggr logging to console/disk features. 
        /// </summary>
        public static readonly bool QuietMode = false;

#if DEBUG
        public static readonly bool DebugMode = true;
#else
        public static readonly bool DebugMode = false;
#endif

        /// <summary>
        /// Enables AnN3x.ModdingLib.Loggr writing log messages to disk.
        /// </summary>
        public static readonly bool WriteLogToDisk = true;

        public static class EndlessMoving
        {
            public static bool Enabled = true;
            public static bool SkipOneTurn = true;
            public static bool OnAllEmpires = true;
        }
    }
}
