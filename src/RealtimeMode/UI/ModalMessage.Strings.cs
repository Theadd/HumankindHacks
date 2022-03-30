using System;
using Amplitude;
using Amplitude.Framework;
using Amplitude.Mercury.UI;
using AnN3x.ModdingLib;

namespace AnN3x.RealtimeMode.UI;

public partial class ModalMessage
{
    private static class Strings
    {
        private static string Gold(string str) => $"<b><c={Colors.GoldenRod}>{str}</c></b>";
        private static string Keyword(string str) => $"<b><c={Colors.LimeGreen}>{str}</c></b>";
        private static string RedKeyword(string str) => $"<b><c={Colors.FireBrick}>{str}</c></b>";

        public static Func<string> MainScreenDescription = (() =>
            @$"Welcome, this is the main screen of the <b>Endless Moving Armies</b> plugin.


Plugin's current {Gold("configuration")} is as follows.

  * {MainScreenItemMovingArmies} 

  * {MainScreenItemSkipOneTurn}


<c={Colors.DarkOrange}>State of <b>Endless Moving Armies</b> plugin is:</c> {PluginStateString}

  * {MainScreenStartStopButtonDescription}

  * {MainScreenOptionsButtonDescription}");

        private static string PluginStateString =>
            "<b>" + (Config.RealtimeMode.Enabled
                ? $"<c={Colors.LimeGreen}>Running</c>"
                : $"<c={Colors.SlateGray}>Inactive</c>") + "</b>";
        
        private static string MainScreenStartStopButtonDescription =>
            (Config.RealtimeMode.Enabled
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
    }
}
