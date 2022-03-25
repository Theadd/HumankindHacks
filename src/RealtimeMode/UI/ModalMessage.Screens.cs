using System;
using Amplitude.Mercury.UI;
using Amplitude.UI;
using AnN3x.ModdingLib;

// ReSharper disable StaticMemberInitializerReferesToMemberBelow

namespace AnN3x.RealtimeMode.UI;

public partial class ModalMessage
{
    public static int EnqueuedScreens { get; private set; } = 0;

    private static readonly Message BaseMessage = new Message()
    {
        Title = "Realtime Mode Hack",
        OnMessageClosed = (Action) (() =>
        {
            if (--EnqueuedScreens >= 1) return;

            CloseMessageBox();
            CloseModalWindow();
            RollbackUIStyle();
        }),
        BlackBackground = false,
        UIMapper = Mapper
    };

    private static Message MainScreen = new(BaseMessage)
    {
        Description = Strings.MainScreenDescription,
        Buttons = new Invokable<MessageBoxButton.Data[]>(() => Config.RealtimeMode.Enabled
            ? new MessageBoxButton.Data[]
            {
                new ModalButton(OptionsButton) { Action = () => ShowScreen(SecondScreen) },
                new ModalButton(StopButton) { Action = () => Config.RealtimeMode.Enabled = false },
                CancelButton
            }
            : new MessageBoxButton.Data[]
            {
                new ModalButton(OptionsButton) { Action = () => ShowScreen(SecondScreen) },
                new ModalButton(StartButton) { Action = () => Config.RealtimeMode.Enabled = true },
                CancelButton
            })
    };

    private static Message SecondScreen = new(BaseMessage)
    {
        Description = "Some other description here for the <b>second screen</b>.",
        Buttons = new[] { YesButton, NoButton, CancelButton }
    };
}
