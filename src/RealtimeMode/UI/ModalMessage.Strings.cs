using System;
using Amplitude;
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
            @$"Welcome, this is the main screen of the <b>RealtimeMode</b> plugin.


Plugin's current {Gold("configuration")} is as follows.

  * <b>Endless Moving Armies</b> is {(!Config.EndlessMoving.Enabled ? Gold("disabled") + "." : (Gold("enabled") + $" {(Config.EndlessMoving.OnAllEmpires ? "and " + Gold("all empires") + " benefit" : "but " + Gold("only you") + " benefit")} from its functionality."))} 

{(Config.RealtimeMode.Enabled ? MainScreenDescriptionOnRuntimeEnabled : MainScreenDescriptionOnRuntimeDisabled)}");

        private static string MainScreenDescriptionOnRuntimeEnabled => 
            $@"<b><c={Colors.DarkOrange}>RealtimeMode's runtime is already running.</c></b>

  * Click the {Keyword("STOP")} button to disable all plugin features at once, being back to normal gameplay again.

  * Click {Keyword("OPTIONS")} to change some RealtimeMode plugin settings.

  * To go back to the game, just click the {RedKeyword("CANCEL")} button.";

        private static string MainScreenDescriptionOnRuntimeDisabled => "";
    }
}
