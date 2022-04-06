using System;
using System.Reflection;
using Amplitude.Mercury.Presentation;
using Amplitude.Mercury.UI;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.Mercury.UI.Windows;
using Amplitude.UI;
using HarmonyLib;
using UnityEngine;

namespace AnN3x.UI;

public static class UIController
{
    private static UIManager _uiManager;
    private static DataUtils _dataUtils;

    public static WindowsManager WindowsManager => WindowsManager.Instance;

    public static UIManager UIManager => _uiManager
        ? _uiManager
        : (_uiManager = Amplitude.Framework.Services.GetService<IUIService>() as UIManager);

    private static readonly FieldInfo DataUtilsField = typeof(Utils).GetField("DataUtils", AccessTools.all);
    public static DataUtils DataUtils => _dataUtils ??= (DataUtils) DataUtilsField.GetValue(null);

    public static UITransform WindowsRoot => WindowsManager.WindowsRoot;

    public static event Action OnGUIHasLoaded;

    public static GUISkin DefaultSkin { get; set; }

    public static bool IsGUILoaded { get; private set; } = false;

    public static float TooltipDelay
    {
        get => UIManager.TooltipDelay;
        set => UIManager.TooltipDelay = value;
    }

    public static bool IsAmplitudeUIVisible
    {
        get => UIManager.IsUiVisible;
        set => UIManager.IsUiVisible = value;
    }

    public static bool AreTooltipsVisible
    {
        get => UIManager.AreTooltipsVisible;
        set => UIManager.AreTooltipsVisible = value;
    }

    public static bool IsCameraSequenceRunning => UIManager.IsCameraSequenceRunning;

    public static bool AnyPendingSequence => Presentation.PresentationCameraSequenceController is
        { AnyPendingSequence: true };

    public static bool GodMode
    {
        get => Amplitude.Mercury.Presentation.GodMode.Enabled;
        set => AccessTools.PropertySetter(typeof(GodMode), "Enabled")?.Invoke(null, new object[] { value });
    }

    public static string GetLocalizedTitle(Amplitude.StaticString uiMapperName, string defaultValue = null) =>
        DataUtils.TryGetLocalizedTitle(uiMapperName, out string title) ? title : defaultValue;

    public static string GetLocalizedDescription(Amplitude.StaticString uiMapperName) =>
        DataUtils?.GetLocalizedDescription(uiMapperName) ?? uiMapperName.ToString();

    public static bool ShouldDrawOnGUI => !IsCameraSequenceRunning && WindowsManager is
        { AnyInGameFullScreenOpened: false, IsGamePaused: false, IsHelpLayerScreenOpened: false };

    private static void InvokeOnGUIHasLoaded()
    {
        IsGUILoaded = true;
        OnGUIHasLoaded += Dummy;
        BepInEx.Utility.TryDo(OnGUIHasLoaded, out _);
        OnGUIHasLoaded -= Dummy;
    }

    private static void Dummy() {}

    private static void Unload() => UIOverlay.Unload();

    public static void Initialize()
    {
        DefaultSkin = Plugin.Assets.Load<GUISkin>("AnN3xUISkin");
        // Unload any previous UIOverlay remaining on scene
        Unload();
        InvokeOnGUIHasLoaded();
    }
}
