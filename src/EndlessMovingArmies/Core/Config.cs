using Amplitude.Framework.Runtime;
using AnN3x.CoreLib;
using BepInEx.Configuration;
using UnityEngine;

namespace AnN3x.EndlessMovingArmies;

internal class Config
{
    /// <summary>
    /// Disables AnN3x.CoreLib.Loggr logging to console/disk features. 
    /// </summary>
#if !NOLOGGR
    public static readonly bool QuietMode = false;
#else
    public static readonly bool QuietMode = true;
#endif
    
#if DEBUG
    public static readonly bool DebugMode = true;
#else
    public static readonly bool DebugMode = false;
#endif

    /// <summary>
    /// Enables AnN3x.CoreLib.Loggr writing log messages to disk.
    /// </summary>
#if !NOLOGGR
    public static readonly bool WriteLogToDisk = true;
#else
    public static readonly bool WriteLogToDisk = false;
#endif

    public static class EndlessMoving
    {
        public static bool Enabled = true;
        public static MovingArmiesMode Mode = MovingArmiesMode.Standard;
        public static bool OnAllEmpires = true;
        public static bool IncludeOtherEmpiresControlledByHuman = true;
        public static bool IsChatNotificationPending = true;

        /// <summary>
        /// Determines the number of empires being processed each loop dividing the number of empires of
        /// all types (major/minor/etc) by this value. So, the time that takes a full cycle of empires to
        /// process will be:
        ///     LoopIterationsPerCollectionOfEmpires * LoopInterval
        /// </summary>
        public static int LoopIterationsPerCollectionOfEmpires => 
            Mode == MovingArmiesMode.Standard ? 10 : 40;

        public static float LoopInterval = .1f;

        /* Section: Aggressive EndlessMovingArmies */

        public static int CyclesToSkipBeforeProcessingMinorEmpires = 12;

        /* END Section */

        /* Section: Standard EndlessMovingArmies */

        /// <summary>
        /// Loops to skip between full cycles, applies only to Standard EndlessMovingArmies.
        /// With .1s as LoopInterval, given a value of 20 means a 2s (20 * .1s) wait time between cycles.
        /// </summary>
        public static int IdleLoopsBetweenCycles = 12;

        public static int PrimaryCyclesToSkip = 5;

        /* END Section */
    }

    public static class Runtime
    {
        private static bool _enabled = false;

        public static bool Enabled
        {
            get => _enabled;
            set
            {
                Plugin.MovingArmiesGameObject.SetActive(value);

                if (Plugin.MovingArmiesGameObject.activeSelf != value)
                    Loggr.Log(new RuntimeException(
                        "Unable to activate the GameObject which controls the Realtime Mode behaviour."));

                _enabled = Plugin.MovingArmiesGameObject.activeSelf;
            }
        }

        public static bool EnableInOnlineSessions = false;

        public static KeyboardShortcut ShowUIKey { get; set; } =
            new KeyboardShortcut(KeyCode.F1, KeyCode.LeftControl);
    }

    public enum MovingArmiesMode
    {
        Standard,
        Aggressive
    }
}
