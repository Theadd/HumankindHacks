using Amplitude.Framework.Runtime;
using AnN3x.ModdingLib;
using BepInEx.Configuration;
using UnityEngine;

namespace AnN3x.RealtimeMode
{
    internal class Config
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
            /// <summary>
            /// Determines the number of empires being processed each loop dividing the provided value by
            /// the number of empires of all types (major/minor). So, the time that takes a full cycle of
            /// empires to process will be:
            ///     LoopIterationsPerCollectionOfEmpires * LoopInterval
            /// </summary>
            public static int LoopIterationsPerCollectionOfEmpires = 10;
            public static float LoopInterval = .1f;
            public static int CyclesToSkipBeforeProcessingMinorEmpires = 7;
        }

        public static class RealtimeMode
        {
            private static bool _enabled = false;
            public static bool Enabled
            {
                get => _enabled;
                set
                {
                    Plugin.RealtimeModeGameObject.SetActive(value);
                    
                    if (Plugin.RealtimeModeGameObject.activeSelf != value)
                        Loggr.Log(new RuntimeException("Unable to activate the GameObject which controls the Realtime Mode behaviour."));

                    _enabled = Plugin.RealtimeModeGameObject.activeSelf;
                }
            }

            public static KeyboardShortcut ShowUIKey { get; set; } =
                new KeyboardShortcut(KeyCode.F1, KeyCode.LeftControl);
        }
    }
}
