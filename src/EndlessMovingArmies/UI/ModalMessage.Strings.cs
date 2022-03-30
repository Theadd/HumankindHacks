using System;
using Amplitude.Framework;
using Amplitude.Mercury.UI;
using AnN3x.ModdingLib;

namespace AnN3x.EndlessMovingArmies.UI;

public partial class ModalMessage
{
    private static class Strings
    {
        private static string Gold(string str) => $"<b><c={Colors.GoldenRod}>{str}</c></b>";
        private static string Keyword(string str) => $"<b><c={Colors.LimeGreen}>{str}</c></b>";
        private static string RedKeyword(string str) => $"<b><c={Colors.FireBrick}>{str}</c></b>";
        private static string OptionName(string str) => $"<b><c={Colors.DarkOrange}>{str}</c></b>";
        private static string Yellow(string str) => $"<c={Colors.Yellow}>{str}</c>";

        #region Main Screen

        public static Func<string> MainScreenDescription = (() =>
            @$"Welcome, this is the main screen of the <b>Endless Moving Armies</b> plugin.


Plugin's current {Gold("configuration")} is as follows.

  * {MainScreenItemMovingArmies} 

  * {MainScreenItemSkipOneTurn}

  * {MainScreenItemEnabledOnline}


<c={Colors.DarkOrange}>State of <b>Endless Moving Armies</b> plugin is:</c> {PluginStateString}

  * {MainScreenStartStopButtonDescription}

  * {MainScreenOptionsButtonDescription}");

        private static string PluginStateString =>
            "<b>" + (Config.Runtime.Enabled
                ? $"<c={Colors.LimeGreen}>Running</c>"
                : $"<c={Colors.SlateGray}>Inactive</c>") + "</b>";

        private static string MainScreenStartStopButtonDescription =>
            (Config.Runtime.Enabled
                ? $"Click the {Keyword("STOP")} button to deactivate all plugin features at once, being back to normal gameplay again."
                : $"Click the {Keyword("START")} button to activate <b>Endless Moving Armies</b> plugin.");

        private static string MainScreenOptionsButtonDescription =>
            $"Click {Keyword("OPTIONS")} to change some plugin settings.";

        private static string MainScreenItemMovingArmies =>
            $"<b>Endless Moving Armies</b> is set to {Gold(Config.EndlessMoving.Mode.ToString())} mode "
            + (Config.EndlessMoving.OnAllEmpires
                ? "and " + Gold("all empires")
                : "but " + (Config.EndlessMoving.IncludeOtherEmpiresControlledByHuman
                    ? Gold("only human empires")
                    : Gold("only you")))
            + " benefit from it.";

        private static string MainScreenItemSkipOneTurn =>
            ((Services.GetService<IUIService>() as UIManager)?.ActivateMandatories ?? false)
                ? $"Your <b>Mandatories</b> settings are {Gold("enabled")} in {Keyword("Settings > UI > Enable Mandatories")}. " +
                  "With <b>Endless Moving Armies</b> active, there will always be armies with movement points left and " +
                  "that would block you from pressing the <b>End Turn</b> button. As a workaround, all armies whose " +
                  $"movement points have been altered, will be set to {Gold("SkipOneTurn")} action after ending its movement action."
                : $"Your <b>Mandatories</b> settings are {Gold("disabled")} in {Keyword("Settings > UI > Enable Mandatories")}.";

        private static string MainScreenItemEnabledOnline =>
            $"Allowed in <b>Online Games</b> is {Gold(Config.Runtime.EnableInOnlineSessions ? "enabled" : "disabled")}.";

        #endregion

        #region Moving Armies Mode Screen

        public static Func<string> MovingArmiesModeScreenDescription = (() =>
            @$"Choose a <b>Moving Behaviour</b> mode between <b>Standard</b> (default) and <b>Aggressive</b>.

" + $"  * {OptionName(Config.MovingArmiesMode.Standard.ToString())} {Yellow("(Recommended)")} works" +
            $"well in all scenarios and has small wait intervals between army movement orders." + @"

" + $"  * {OptionName(Config.MovingArmiesMode.Aggressive.ToString())} removes those small wait intervals " +
            $"between army movements by continuously refilling their movement points. Has higher CPU usage " +
            $"and makes enemy armies almost impossible to target when this mode is applied to " +
            $"{Gold("all empires")} since they hardly take a break." + @$"

Current value is set to {Gold(Config.EndlessMoving.Mode.ToString())}.");

        #endregion

        #region Affected Empires Screen

        public static string TargetGroupOfEmpiresString => Config.EndlessMoving.OnAllEmpires
            ? "All Empires"
            : Config.EndlessMoving.IncludeOtherEmpiresControlledByHuman
                ? "Human Empires"
                : "Local Empire";

        public static Func<string> AffectedEmpiresScreenDescription = (() =>
            @$"Choose the <b>Target Group of Empires</b> where <b>Endless Moving Armies</b> will be applied.

" + $"  * {OptionName("All Empires")} affects <b>All Major and Minor Empires</b>, whether they're " +
            $"controlled by Human or by the AI." + @"

" + $"  * {OptionName("Human Empires")} only affects <b>Major Empires controlled by Humans</b>." + @"

" + $"  * {OptionName("Local Empire")} only affects <b>Your Empire</b>." + @$"

Current value is set to {Gold(TargetGroupOfEmpiresString)}.");
        
        #endregion
        
        #region Enabled In Online Games Screen
        
        public static Func<string> EnabledInOnlineGamesScreenDescription = (() =>
            @$"Allow <b>Endless Moving Armies</b> in <b>Online Games</b>?

When enabled and within an <b>online game</b>, it will send a chat message to notify the other players about it.

Current value is set to {Gold(Config.Runtime.EnableInOnlineSessions ? "enabled" : "disabled")}.");

        #endregion
        
        #region Advanced Options End Screen
        
        public static Func<string> AdvancedOptionsEndScreenDescription = (() =>
            @$"<b>That's all.</b>

<c={Colors.DarkOrange}>Current state of <b>Endless Moving Armies</b> plugin is:</c> {PluginStateString}");

        #endregion
        
        
    }
}
