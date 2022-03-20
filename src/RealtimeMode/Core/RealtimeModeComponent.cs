using System;
using System.Collections;
using AnN3x.HumankindLib;
using AnN3x.ModdingLib;
using AnN3x.RealtimeMode.Armies;
using UnityEngine;

namespace AnN3x.RealtimeMode;

public class RealtimeModeComponent : MonoBehaviour
{
    private void OnEnable()
    {
        Loggr.Log("STARTING Loop() COROUTINE @ RealtimeModeComponent", ConsoleColor.Magenta);
        StartCoroutine(Loop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator Loop()
    {
        for(;;)
        {
            yield return new WaitForSeconds(Config.EndlessMoving.LoopInterval);
            if (HumankindGame.IsInGame)
                ArmyController.Run();
        }
    }
}
