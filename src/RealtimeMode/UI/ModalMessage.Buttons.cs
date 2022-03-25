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
        false);

    private static readonly MessageBoxButton.Data StartButton = new(
        "Start",
        "",
        () => true,
        new StaticString("StartButton"),
        false);
    
    private static readonly MessageBoxButton.Data StopButton = new(
        "Stop",
        "",
        () => true,
        new StaticString("StopButton"),
        false);
    
    private static readonly MessageBoxButton.Data YesButton = new(
        "Yes",
        "",
        () => true,
        new StaticString("YesButton"),
        false);

    private static readonly MessageBoxButton.Data NoButton = new(
        "No",
        "",
        () => true,
        new StaticString("NoButton"),
        false);

    private static readonly MessageBoxButton.Data CancelButton = new(
        "Cancel",
        "Close this window without applying any change.",
        () => true,
        new StaticString("CancelButton"),
        true) { IsRed = true };

    private static readonly MessageBoxButton.Data NoneButton = new(MessageBox.Choice.None, null);
}
