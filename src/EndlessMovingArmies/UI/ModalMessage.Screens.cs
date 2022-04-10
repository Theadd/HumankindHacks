using System;
using Amplitude.Mercury.UI;
using AnN3x.CoreLib;

// ReSharper disable StaticMemberInitializerReferesToMemberBelow

namespace AnN3x.EndlessMovingArmies.UI;

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
        Buttons = new Invokable<MessageBoxButton.Data[]>(() => Config.Runtime.Enabled
            ? new MessageBoxButton.Data[]
            {
                new ModalButton(OptionsButton) { Action = () => ShowScreen(ChooseMovingArmiesModeScreen) },
                new ModalButton(StopButton) { Action = () => Config.Runtime.Enabled = false },
                CancelButton
            }
            : new MessageBoxButton.Data[]
            {
                new ModalButton(OptionsButton) { Action = () => ShowScreen(ChooseMovingArmiesModeScreen) },
                new ModalButton(StartButton) { Action = () => Config.Runtime.Enabled = true },
                CancelButton
            })
    };

    private static Message ChooseMovingArmiesModeScreen = new(BaseMessage)
    {
        Description = Strings.MovingArmiesModeScreenDescription,
        Buttons = new MessageBoxButton.Data[]
        {
            new ModalButton(AggressiveModeButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.Mode = Config.MovingArmiesMode.Aggressive;
                    ShowScreen(ChooseAffectedEmpiresScreen);
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
        Description = Strings.AffectedEmpiresScreenDescription,
        Buttons = new MessageBoxButton.Data[]
        {
            new ModalButton(AllEmpiresButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.OnAllEmpires = true;
                    ShowScreen(EnabledInOnlineGamesScreen);
                }
            },
            new ModalButton(HumanEmpiresButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.OnAllEmpires = false;
                    Config.EndlessMoving.IncludeOtherEmpiresControlledByHuman = true;
                    ShowScreen(EnabledInOnlineGamesScreen);
                }
            },
            new ModalButton(LocalEmpireButton)
            {
                Action = () =>
                {
                    Config.EndlessMoving.OnAllEmpires = false;
                    Config.EndlessMoving.IncludeOtherEmpiresControlledByHuman = false;
                    ShowScreen(EnabledInOnlineGamesScreen);
                }
            },
            // new ModalButton(BackButton) { Action = () => ShowScreen(ChooseMovingArmiesModeScreen) },
        }
    };

    private static Message EnabledInOnlineGamesScreen = new(BaseMessage)
    {
        Description = Strings.EnabledInOnlineGamesScreenDescription,
        Buttons = new MessageBoxButton.Data[]
        {
            new ModalButton(YesButton)
            {
                Action = () =>
                {
                    Config.Runtime.EnableInOnlineSessions = true;
                    ShowScreen(AdvancedOptionsEndScreen);
                }
            },
            new ModalButton(NoButton)
            {
                Action = () =>
                {
                    Config.Runtime.EnableInOnlineSessions = false;
                    ShowScreen(AdvancedOptionsEndScreen);
                }
            },
            new ModalButton(BackButton) { Action = () => ShowScreen(ChooseAffectedEmpiresScreen) },
        }
    };

    private static Message AdvancedOptionsEndScreen = new(BaseMessage)
    {
        Description = Strings.AdvancedOptionsEndScreenDescription,
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
