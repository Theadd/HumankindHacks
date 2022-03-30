using System;
using System.Collections;
using AnN3x.HumankindLib;
using AnN3x.ModdingLib;
using AnN3x.RealtimeMode.Armies;
using UnityEngine;

namespace AnN3x.RealtimeMode;

public class RealtimeModeComponent : MonoBehaviour
{
    private static int LastKnownTurn { get; set; }
    private static bool IsOnlineGame { get; set; }

    private void OnEnable() => StartCoroutine(Loop());

    private void OnDisable() => StopAllCoroutines();

    private static bool IsNewTurn()
    {
        var currentTurn = HumankindGame.Turn;

        if (LastKnownTurn == currentTurn || currentTurn == 0)
            return false;

        LastKnownTurn = currentTurn;
        IsOnlineGame = HumankindGame.IsOnlineGame;
        return true;
    }

    private static string Gold(string str) => $"<b><c={Colors.GoldenRod}>{str}</c></b>";

    private static void SendNotificationChatMessage()
    {
        var prev = Colors.PrefixWithHash;
        Colors.PrefixWithHash = false;
        var msg =
            $"<b>Endless Moving Armies</b> is set to {Gold(Config.EndlessMoving.Mode.ToString())} mode "
            + (Config.EndlessMoving.OnAllEmpires
                ? "and " + Gold("all empires")
                : "but " + (Config.EndlessMoving.IncludeOtherEmpiresControlledByHuman
                    ? Gold("only human empires")
                    : Gold("only I")))
            + " benefit from it.";
        Colors.PrefixWithHash = prev;

        if (HumankindGame.TrySendChatMessage(msg))
        {
            Config.EndlessMoving.IsChatNotificationPending = false;
        }
    }

    IEnumerator Loop()
    {
        for (;;)
        {
            yield return new WaitForSeconds(Config.EndlessMoving.LoopInterval);
            if (HumankindGame.IsInGame && Config.EndlessMoving.Enabled)
            {
                var isNewTurn = IsNewTurn();

                if (IsOnlineGame && !Config.RealtimeMode.EnableInOnlineSessions)
                    continue;

                if (IsOnlineGame && Config.EndlessMoving.IsChatNotificationPending)
                    SendNotificationChatMessage();

                switch (Config.EndlessMoving.Mode)
                {
                    case Config.MovingArmiesMode.Standard:
                        StandardEndlessMoving.Run(isNewTurn || HumankindGame.IsUILockedByEndTurn);
                        break;
                    case Config.MovingArmiesMode.Aggressive:
                        AggressiveEndlessMoving.Run(isNewTurn || HumankindGame.IsUILockedByEndTurn);
                        break;
                }
            }
        }
    }
}
