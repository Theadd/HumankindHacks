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
        Title = "Endless Moving Armies",
        OnMessageClosed = (Action) (() =>
        {
            if (--EnqueuedScreens >= 1) return;

            Config.EndlessMoving.IsChatNotificationPending = true;
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
                new ModalButton(OptionsButton) { Action = () => ShowScreen(ChooseMovingArmiesModeScreen) },
                new ModalButton(StopButton) { Action = () => Config.RealtimeMode.Enabled = false },
                CancelButton
            }
            : new MessageBoxButton.Data[]
            {
                new ModalButton(OptionsButton) { Action = () => ShowScreen(ChooseMovingArmiesModeScreen) },
                new ModalButton(StartButton) { Action = () => Config.RealtimeMode.Enabled = true },
                CancelButton
            })
    };

    private static Message ChooseMovingArmiesModeScreen = new(BaseMessage)
    {
        Buttons = new MessageBoxButton.Data[]
        {
            new ModalButton(AggressiveModeButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.Mode = Config.MovingArmiesMode.Aggressive;
                    ShowScreen(AdvancedOptionsEndScreen);
                }
            },
            new ModalButton(StandardModeButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.Mode = Config.MovingArmiesMode.Standard;
                    ShowScreen(ChooseAffectedEmpiresScreen);
                }
            },
            new ModalButton(BackButton) { Action = () => ShowScreen(MainScreen) },
        }
    };

    private static Message ChooseAffectedEmpiresScreen = new(BaseMessage)
    {
        Description = (Func<string>) (() =>
            $"Config.EndlessMoving.OnAllEmpires = <b>{Config.EndlessMoving.OnAllEmpires}</b>."),
        Buttons = new MessageBoxButton.Data[]
        {
            new ModalButton(AllEmpiresButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.OnAllEmpires = true;
                    ShowScreen(AdvancedOptionsEndScreen);
                }
            },
            new ModalButton(HumanEmpiresButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.OnAllEmpires = false;
                    ShowScreen(AdvancedOptionsEndScreen);
                }
            },
            new ModalButton(BackButton) { Action = () => ShowScreen(ChooseMovingArmiesModeScreen) },
        }
    };

    private static Message AdvancedOptionsEndScreen = new(BaseMessage)
    {
        Description = "DONE.",
        Buttons = new MessageBoxButton.Data[]
        {
            new ModalButton(BackButton)
            {
                Title = "Back To Main Screen",
                Action = () => ShowScreen(MainScreen)
            },
            CloseButton
        }
    };
}
