using System.Linq;
using System.Reflection;
using Amplitude;
using Amplitude.Framework;
using Amplitude.Mercury.UI;
using Amplitude.Mercury.UI.Helpers;
using Amplitude.UI;
using Amplitude.UI.Layouts;
using Amplitude.UI.Renderers;
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
    public static readonly FieldInfo MsgBox = R.GetField<MessageModalWindow>("messageBox");
    public static readonly FieldInfo MinimumWidth = R.GetField<MessageBox>("minimumWidth");
    public static readonly FieldInfo TitleLabel = R.GetField<MessageBox>("title");
    public static readonly FieldInfo DescriptionLabel = R.GetField<MessageBox>("description");
    public static readonly FieldInfo ImageComponent = R.GetField<MessageBox>("image");

    public static readonly string MapperName = "RealtimeModeModalWindow";

    public static Texture2D Image => _image ??= AssetHunter.GetAssetBundlesContaining("society_background")
        .FirstOrDefault()?.LoadAsset<Texture2D>("society_background");

    public static UIMapper Mapper => _uiMapper ??= CreateUIMapper();

    public static MessageModalWindow Window => WindowsUtils.GetWindow<MessageModalWindow>();

    public static void ApplyUIStyle()
    {
        MessageBox messageBox = (MessageBox) MsgBox.GetValue(Window);
        Loggr.Debug($"MinimumWidth = {(float) MinimumWidth.GetValue(messageBox)}");
        MinimumWidth.SetValue(messageBox, 510f);
        UIImage image = (UIImage) ImageComponent.GetValue(messageBox);
        image.transform.SetAsFirstSibling();
        UILabel title = (UILabel) TitleLabel.GetValue(messageBox);
        UILabel description = (UILabel) DescriptionLabel.GetValue(messageBox);
        title.transform.SetAsFirstSibling();
        var anchor = image.UITransform.TopAnchor;
        Loggr.Debug(
            $"image.UITransform.TopAnchor = ({anchor.Attach}, {anchor.Percent}, {anchor.Margin}, {anchor.Offset})");
        image.UITransform.TopAnchor = new UIBorderAnchor(true, 0, title.UITransform.Height, 0);
        Loggr.Debug("description.Alignment = Alignment.CenterCenter");
        description.Alignment = new Alignment();
        var table = title.UITransform.Parent.GetComponent<UITable1D>();
        Loggr.Debug($"table.Spacing =  {table.Spacing}");
        table.Spacing = 0;
        Loggr.Debug($"messageBox.UITransform.Width = {messageBox.UITransform.Width}");
        messageBox.UITransform.Width = 510f;

        description.AutoAdjustFontSizeMin = 15;
        description.Margins = new RectMargins(24f, 24f, 24f, 48f);
        description.WordWrap = true;
        description.Justify = true;
        description.InterpreteRichText = true;
        description.InterLineAdditionalSpacing = 5;

        var boxed = description.gameObject.GetComponent<SquircleBackgroundWidget>();

        if (boxed == null)
        {
            boxed = description.gameObject.AddComponent<SquircleBackgroundWidget>();
            boxed.OuterBorderColor = Color.clear;
            boxed.BorderColor = Color.clear;
            boxed.CornerRadius = 0f;
        }

        boxed.BackgroundColor = new Color(0, 0, 0, 0.7f);

        messageBox.RefreshNow();
    }

    public static void RollbackUIStyle()
    {
        MessageBox messageBox = (MessageBox) MsgBox.GetValue(Window);
        // Loggr.Debug($"MinimumWidth = {(float)MinimumWidth.GetValue(messageBox)}");
        MinimumWidth.SetValue(messageBox, 760f);
        UIImage image = (UIImage) ImageComponent.GetValue(messageBox);
        UILabel title = (UILabel) TitleLabel.GetValue(messageBox);
        UILabel description = (UILabel) DescriptionLabel.GetValue(messageBox);
        image.transform.SetAsFirstSibling();
        description.transform.SetAsFirstSibling();
        title.transform.SetAsFirstSibling();
        // var anchor = image.UITransform.TopAnchor;
        // Loggr.Debug($"image.UITransform.TopAnchor = ({anchor.Attach}, {anchor.Percent}, {anchor.Margin}, {anchor.Offset})");
        image.UITransform.TopAnchor = new UIBorderAnchor(false, 0, 0, 0);
        // Loggr.Debug("description.Alignment = Alignment.CenterCenter");
        description.Alignment = Alignment.CenterCenter;
        var table = title.UITransform.Parent.GetComponent<UITable1D>();
        // Loggr.Debug($"table.Spacing = {table.Spacing}");
        table.Spacing = 32f;
        // Loggr.Debug($"messageBox.UITransform.Width = {messageBox.UITransform.Width}");
        messageBox.UITransform.Width = 760f;

        description.AutoAdjustFontSizeMin = 16;
        description.Margins = new RectMargins(64f, 64f, 0, 0);
        description.WordWrap = true;
        description.Justify = false;
        description.InterLineAdditionalSpacing = 3;

        var boxed = description.gameObject.GetComponent<SquircleBackgroundWidget>();
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
        uiMappers.Touch(uiMapper);

        return uiMappers.GetValue(new StaticString(MapperName));
    }

    public static void Show()
    {
        _messageToken = MessageModalWindow.ShowMessage(MainScreen);
        ApplyUIStyle();
    }
}
