using System;
using System.Linq;
using System.Reflection;
using Amplitude;
using Amplitude.Framework;
using Amplitude.Graphics.Text;
using Amplitude.Mercury.UI;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.UI;
using Amplitude.UI.Layouts;
using Amplitude.UI.Renderers;
using Amplitude.UI.Windows;
using AnN3x.HumankindLib;
using AnN3x.HumankindLib.Reflection;
using AnN3x.ModdingLib;
using UnityEngine;

namespace AnN3x.RealtimeMode.UI;

public partial class ModalMessage
{
    private static Texture2D _image;
    private static UITexture _uiTexture = UITexture.None;
    private static UIMapper _uiMapper;
    private static MessageModalWindow.Message _message = MessageModalWindow.Message.Empty;
    private static int _messageToken;
    private static Color BoxedBgColor { get; } = Colors.SeaShell.alpha(0.35f);
    public static readonly FieldInfo MsgBox = R.GetField<MessageModalWindow>("messageBox");
    public static readonly FieldInfo MinimumWidth = R.GetField<MessageBox>("minimumWidth");
    public static readonly FieldInfo TitleLabel = R.GetField<MessageBox>("title");
    public static readonly FieldInfo DescriptionLabel = R.GetField<MessageBox>("description");
    public static readonly FieldInfo ImageComponent = R.GetField<MessageBox>("image");
    public static readonly FieldInfo ButtonsTable = R.GetField<MessageBox>("buttonsTable");
    public static readonly FieldInfo AllButtons = R.GetField<MessageBox>("allButtons");

    public static readonly string MapperName = "RealtimeModeMainWindow";

    public static Texture2D Image => _image ??= AssetHunter.GetAssetBundlesContaining("society_background")
        .FirstOrDefault()?.LoadAsset<Texture2D>("society_background");

    public static UIMapper Mapper => _uiMapper ??= CreateUIMapper();

    public static MessageModalWindow Window => WindowsUtils.GetWindow<MessageModalWindow>();

    public static void CloseModalWindow(bool instant = true)
    {
        if (Window.Shown)
            WindowsUtils.HideWindow((UIWindow) Window, instant);
    }

    public static void ApplyUIStyle()
    {
        MessageBox messageBox = (MessageBox) MsgBox.GetValue(Window);

        UITable1D buttonsTable = (UITable1D) ButtonsTable.GetValue(messageBox);
        MessageBoxButton[] allButtons = (MessageBoxButton[]) AllButtons.GetValue(messageBox);
        UIImage image = (UIImage) ImageComponent.GetValue(messageBox);
        UILabel title = (UILabel) TitleLabel.GetValue(messageBox);
        UILabel description = (UILabel) DescriptionLabel.GetValue(messageBox);

        MinimumWidth.SetValue(messageBox, 510f);
        image.transform.SetAsFirstSibling();
        title.transform.SetAsFirstSibling();
        image.UITransform.TopAnchor = new UIBorderAnchor(true, 0, title.UITransform.Height, 0);
        description.Alignment = new Alignment();
        var table = title.UITransform.Parent.GetComponent<UITable1D>();
        table.Spacing = 0;
        messageBox.UITransform.Width = 510f;

        description.AutoAdjustFontSizeMin = 16;
        description.Margins = new RectMargins(24f, 24f, 24f, 48f);
        description.WordWrap = true;
        description.Justify = false;
        description.InterpreteRichText = true;
        description.InterLetterAdditionalSpacing = 0;
        description.InterLineAdditionalSpacing = 7;
        description.InterParagraphAdditionalSpacing = -6;
        description.RenderingMode = FontRenderingMode.DistanceField;

        buttonsTable.UITransform.BottomAnchor = new UIBorderAnchor(true, 1f, 0, 0);
        buttonsTable.UITransform.LeftAnchor = new UIBorderAnchor(true, 0, 0, 0);
        buttonsTable.UITransform.RightAnchor = new UIBorderAnchor(true, 1f, 0, 0);
        buttonsTable.AutoResize = false;
        buttonsTable.Spacing = 0;
        buttonsTable.EvenlySpaced = true;
        buttonsTable.Margin = 16f; // TODO

        foreach (var button in allButtons)
        {
            button.UITransform.PivotYAnchor = new UIPivotAnchor(true, 0, 16f, 16f,
                /* TODO */ 0);
        }

        var boxed = buttonsTable.gameObject.GetComponent<SquircleBackgroundWidget>();

        if (boxed == null)
        {
            boxed = buttonsTable.gameObject.AddComponent<SquircleBackgroundWidget>();
            boxed.OuterBorderColor = Color.clear;
            boxed.BorderColor = Color.clear;
            boxed.CornerRadius = 0f;
        }

        boxed.BackgroundColor = BoxedBgColor;

        messageBox.RefreshNow();
    }

    public static void RollbackUIStyle()
    {
        MessageBox messageBox = (MessageBox) MsgBox.GetValue(Window);

        UITable1D buttonsTable = (UITable1D) ButtonsTable.GetValue(messageBox);
        MessageBoxButton[] allButtons = (MessageBoxButton[]) AllButtons.GetValue(messageBox);
        UIImage image = (UIImage) ImageComponent.GetValue(messageBox);
        UILabel title = (UILabel) TitleLabel.GetValue(messageBox);
        UILabel description = (UILabel) DescriptionLabel.GetValue(messageBox);

        MinimumWidth.SetValue(messageBox, 760f);
        image.transform.SetAsFirstSibling();
        description.transform.SetAsFirstSibling();
        title.transform.SetAsFirstSibling();
        image.UITransform.TopAnchor = new UIBorderAnchor(false, 0, 0, 0);
        description.Alignment = Alignment.CenterCenter;
        var table = title.UITransform.Parent.GetComponent<UITable1D>();
        table.Spacing = 32f;
        messageBox.UITransform.Width = 760f;

        description.AutoAdjustFontSizeMin = 16;
        description.Margins = new RectMargins(64f, 64f, 0, 0);
        description.WordWrap = true;
        description.Justify = false;
        description.InterLetterAdditionalSpacing = 1;
        description.InterLineAdditionalSpacing = 3;
        description.InterParagraphAdditionalSpacing = 0;
        description.RenderingMode = FontRenderingMode.Raster;

        buttonsTable.UITransform.BottomAnchor = new UIBorderAnchor(false, 1f, 0, 0);
        buttonsTable.UITransform.LeftAnchor = new UIBorderAnchor(false, 0, 0, 0);
        buttonsTable.UITransform.RightAnchor = new UIBorderAnchor(false, 1f, 0, 0);
        buttonsTable.AutoResize = true;
        buttonsTable.Spacing = 32f;
        buttonsTable.EvenlySpaced = false;
        buttonsTable.Margin = 12f;

        foreach (var button in allButtons)
        {
            button.UITransform.PivotYAnchor = new UIPivotAnchor(false, 1f, 0, 0, 0);
        }

        var boxed = buttonsTable.gameObject.GetComponent<SquircleBackgroundWidget>();
        if (boxed != null)
        {
            boxed.BackgroundColor = Color.clear;
        }

        messageBox.RefreshNow();
    }

    public static UITexture CreateUITexture(Texture2D texture) => new UITexture(
        Amplitude.Framework.Guid.NewGuid(),
        UITextureFlags.AlphaStraight,
        UITextureColorFormat.Srgb,
        (Texture) texture);

    private static UIMapper CreateUIMapper()
    {
        _uiTexture = CreateUITexture(Image);

        var uiMappers = Databases.GetDatabase<UIMapper>();
        var uiMapper = ScriptableObject.CreateInstance<UIMapper>();

        uiMapper.name = MapperName;
        uiMapper.XmlSerializableName = MapperName;
        uiMapper.Title = "REALTIME MODE HACK";
        uiMapper.Description =
            "Here goes <b>plugin description</b>. Here goes plugin description. <c=ECEC00>Here goes plugin description</c>. Here goes plugin description.\n\nPlus some more info over here also.";
        uiMapper.Images = new[]
        {
            new UIMapper.Image(_uiTexture, DataUtils.ImageSize.QRCode.ToString())
        };
        uiMapper.Initialize();
        // uiMappers.Touch(uiMapper);

        return uiMapper;
        //return uiMappers.GetValue(new StaticString(MapperName));
    }

    public static void Show()
    {
        if (EnqueuedScreens > 0)
        {
            CloseMessageBox();
        }
        else
        {
            CloseMessageBox();
            ShowScreen(MainScreen);
        }
    }

    public static void ShowScreen(Message message)
    {
        var prev = Colors.PrefixWithHash;
        Colors.PrefixWithHash = false;
        MessageModalWindow.Message msg = message;

        _messageToken = MessageModalWindow.ShowMessage(ref msg);
        EnqueuedScreens++;

        ApplyUIStyle();
        Colors.PrefixWithHash = prev;
    }

    public static void CloseMessageBox()
    {
        try
        {
            var modalWindow = Window;
            if (MsgBox.GetValue(modalWindow) is MessageBox { Shown: true } messageBox)
            {
                messageBox.TryDismiss();
                modalWindow.OnBoxCloseNonPopupsRequested();
                modalWindow.OnBoxCloseRequested();
            }
        }
        catch (Exception e)
        {
            Loggr.Log(e);
        }
    }
}
