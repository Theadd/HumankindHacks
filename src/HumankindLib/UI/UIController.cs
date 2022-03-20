using System;
using System.Collections.Generic;
using System.Linq;
using Amplitude;
using Amplitude.Framework;
using Amplitude.Framework.Asset;
// using Amplitude.Framework.Asset;
using Amplitude.Mercury.Presentation;
using Amplitude.Mercury.UI.Windows;
using Amplitude.UI;
using Amplitude.UI.Windows;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.Mercury.UI;
using AnN3x.HumankindLib.Reflection;
using AnN3x.ModdingLib;
using HarmonyLib;
using UnityEngine;
using AssetBundle = UnityEngine.AssetBundle;

namespace AnN3x.HumankindLib.UI;

public class UIController
{
    private static UIManager _uiManager;
    private static DataUtils _dataUtils;

    public static WindowsManager WindowsManager => WindowsManager.Instance;
    
    public static UIManager UIManager => _uiManager ? _uiManager
        : (_uiManager = Amplitude.Framework.Services.GetService<IUIService>() as UIManager);
    
    public static DataUtils DataUtils => _dataUtils ??= (DataUtils) R.DataUtils.GetValue(null);
    
    public static List<GameWindow> AllGameWindows => (List<GameWindow>) R.AllGameWindows.GetValue(WindowsManager);
    
    public UITransform WindowsRoot => WindowsManager.WindowsRoot;
    
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

    public static bool AnyPendingSequence => Presentation.PresentationCameraSequenceController is { AnyPendingSequence: true };
        
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
    
    public static void Test()
    {
        Loggr.Debug("AnyInGameFullScreenOpened = " + UIController.WindowsManager.AnyInGameFullScreenOpened);
        Loggr.Debug("IsGamePaused = " + UIController.WindowsManager.IsGamePaused);
        Loggr.Debug("IsHelpLayerScreenOpened = " + UIController.WindowsManager.IsHelpLayerScreenOpened);
        
        foreach (var window in AllGameWindows)
        {
            var prefix = window.Shown ? "%GREEN%VISIBLE%DEFAULT%" : "       ";
            var group = (R.GetPropValue<UIWindowsGroup>(window, "Group"))?.GetType().Name ?? "";
            Loggr.Log($"{prefix} {window.GetType().Name} %GRAY%{group}", ConsoleColor.DarkYellow);
            throw new TimeoutException("Some TimeoutException message...");
        }
    }

    public static void TestGUISkins()
    {
        // AssetBundle
        // Amplitude.Mercury.UI.MessageModalWindow modal;
        try
        {
            Test();
        }
        catch (Exception e)
        {
            Loggr.Log(e);
        }
        
        var msg = new MessageModalWindow.Message()
        {
            // Title = "%ModalConfirmDeclareWarAndOrderTitle",
            Title = "HEllo WoRLD!!!",
            Description = "Description <b>goes here</b>, <c=F39C12>OH YEAH</c>!\n <c=F39C12>OH YEAH</c>!",
            BlackBackground = false,
            Buttons = new MessageBoxButton.Data[]
            {
                new("Button Title", "Button Description", () => true, new StaticString("ButtonChoiceName"), false),
                
            }
            // TimeoutInSeconds = 6f,
            // UIMapper = UIController.DataUtils.GetUIMapper(new Amplitude.StaticString("ConstructibleCostModifier_HarborCostReductionFromJetty")),
            /*Buttons = new MessageBoxButton.Data[4]
            {
                new(MessageBox.Choice.No, (Func<bool>) null, true),
                new MessageBoxButton.Data(MessageBox.Choice.No, (Func<bool>) null, true),
                new MessageBoxButton.Data(MessageBox.Choice.Cancel, (Func<bool>) null, true),
                new MessageBoxButton.Data(MessageBox.Choice.Yes, new Func<bool>(OnModalButton_Yes))
            }*/
        };
        
        var uiMappers = Databases.GetDatabase<UIMapper>();
        //uiMappers.Touch()
        // MessageBox
        
        /*UITexture tex = new UITexture(
            Amplitude.Framework.Guid.NewGuid(), 
            UITextureFlags.AlphaStraight, 
            UITextureColorFormat.Srgb, 
            (Texture) this.mapPictureUnityTexture);*/
    }
    
    public static AssetBundle[] GetAssetBundleContaining(string assetPath)
    {
        var bundles = AssetBundle.GetAllLoadedAssetBundles()
            .Where(b => b.Contains(assetPath))
            .ToArray();

        return bundles;
    }
}

