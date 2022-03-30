using System;
using Amplitude;
using Amplitude.Mercury.UI;
using AnN3x.ModdingLib;

namespace AnN3x.EndlessMovingArmies.UI;

public class ModalButton
{
    public static readonly Action DefaultAction = () => { };
    public string Title { get; set; }
    public string Description { get; set; }
    public Func<bool> OnClick { get; set; }
    public bool IsDismiss { get; set; }
    public bool IsRed { get; set; }
    public StaticString ChoiceName { get; set; }
    public Action Action { get; set; } = DefaultAction;
    public bool InheritOnClickActions { get; set; } = false;

    public ModalButton(MessageBoxButton.Data copyFrom)
    {
        Title = copyFrom.Title;
        Description = copyFrom.Description;
        OnClick = copyFrom.OnClick;
        IsDismiss = copyFrom.IsDismiss;
        IsRed = copyFrom.IsRed;
        ChoiceName = copyFrom.ChoiceName;
    }

    public MessageBoxButton.Data ToButtonData()
    {
        Func<bool> action = () =>
        {
            bool returnValue = true;

            if (InheritOnClickActions)
                returnValue = OnClick() && returnValue;

            return Action.TryInvoke() && returnValue;
        };

        var data = new MessageBoxButton.Data()
        {
            Title = Title,
            Description = Description,
            OnClick = action,
            IsDismiss = IsDismiss,
            IsRed = IsRed,
            ChoiceName = ChoiceName
        };

        return data;
    }

    public static implicit operator MessageBoxButton.Data(ModalButton button) => button.ToButtonData();
    public static implicit operator ModalButton(MessageBoxButton.Data data) => new ModalButton(data);
}
