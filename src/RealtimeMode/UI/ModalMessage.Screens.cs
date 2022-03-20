using System;
using Amplitude.Mercury.UI;
using AnN3x.ModdingLib;

namespace AnN3x.RealtimeMode.UI;

public partial class ModalMessage
{
    private static int _count;
    private static int Foo() => _count++;

    private static void TestMe()
    {
        Invokable<int> num = (Func<int>)(() => 5);
        Invokable<int> num2 = (Invokable<int>) Foo;
    }

    private static readonly MessageModalWindow.Message BaseScreen = new Message()
    {
        Title = "Message",
        OnMessageClosed = (Action)RollbackUIStyle,
        BlackBackground = new Invokable<bool>(() => false),
        TestInt = (Func<int>)Foo
        // TestInt = Foo,
        // BlackBackground = () => false,
    };

    private static MessageModalWindow.Message MainScreen => new Message(BaseScreen)
    {
        Description = (Func<string>) (() => $"Some description here, count = {_count}."),
        Buttons = new [] { OptionsButton, YesButton, CancelButton },
        UIMapper = Mapper
    };
}
