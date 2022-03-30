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
    
    private static readonly MessageBoxButton.Data CloseButton = new(
        "Close",
        "",
        () => true,
        new StaticString("CloseButton"),
        true);
    
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
    
    private static readonly MessageBoxButton.Data AllEmpiresButton = new(
        "All Empires",
        "",
        () => true,
        new StaticString("AllEmpiresButton"),
        false);
    
    private static readonly MessageBoxButton.Data HumanEmpiresButton = new(
        "Human Empires Only",
        "",
        () => true,
        new StaticString("HumanEmpiresButton"),
        false);
    
    private static readonly MessageBoxButton.Data StandardModeButton = new(
        "Standard Mode",
        "",
        () => true,
        new StaticString("StandardModeButton"),
        false);
    
    private static readonly MessageBoxButton.Data AggressiveModeButton = new(
        "Aggressive Mode",
        "",
        () => true,
        new StaticString("AggressiveModeButton"),
        false);

    private static readonly MessageBoxButton.Data BackButton = new(
        "Back",
        "Go back.",
        () => true,
        new StaticString("BackButton"),
        false) { IsRed = true };
}
