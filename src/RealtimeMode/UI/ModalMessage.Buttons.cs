using Amplitude;
using Amplitude.Mercury.UI;

namespace AnN3x.RealtimeMode.UI;

public partial class ModalMessage
{
    private static readonly MessageBoxButton.Data OptionsButton = new(
        "Options",
        "Show advanced settings.",
        () => true,
        new StaticString("OptionsButton"),
        true);

    private static readonly MessageBoxButton.Data YesButton = new(
        "Yes",
        "",
        () => true,
        new StaticString("YesButton"),
        true);

    private static readonly MessageBoxButton.Data NoButton = new(
        "No",
        "",
        () => true,
        new StaticString("NoButton"),
        true);

    private static readonly MessageBoxButton.Data CancelButton = new(
        "Cancel",
        "Close this window without applying any change.",
        () => true,
        new StaticString("CancelButton"),
        true) { IsRed = true };
}
