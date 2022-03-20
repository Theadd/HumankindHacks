﻿using System;
using Amplitude.Mercury.UI;
using Amplitude.UI;
using AnN3x.ModdingLib;

namespace AnN3x.RealtimeMode.UI;

internal class Message
{
    public Invokable<String> Title { get; set; }
    public Invokable<string> Description { get; set; }
    public Invokable<UIMapper> UIMapper { get; set; }
    public Invokable<MessageBoxButton.Data[]> Buttons { get; set; }
    public Invokable<System.Action> OnMessageClosed { get; set; }
    public Invokable<float> TimeoutInSeconds { get; set; }
    public Invokable<bool> BlackBackground { get; set; }
    // TODO: remove
    public Invokable<int> TestInt { get; set; }
    
    public Message() : this(MessageModalWindow.Message.Empty) { }

    public Message(MessageModalWindow.Message copyFrom)
    {
        Title = copyFrom.Title;
        Description = copyFrom.Description;
        UIMapper = copyFrom.UIMapper;
        Buttons = copyFrom.Buttons;
        OnMessageClosed = copyFrom.OnMessageClosed;
        TimeoutInSeconds = copyFrom.TimeoutInSeconds;
        BlackBackground = copyFrom.BlackBackground;
    }
    
    public MessageModalWindow.Message ToModalWindowMessage()
    {
        return new MessageModalWindow.Message()
        {
            Title = Title,
            Description = Description,
            UIMapper = UIMapper,
            Buttons = Buttons,
            OnMessageClosed = OnMessageClosed,
            TimeoutInSeconds = TimeoutInSeconds,
            BlackBackground = BlackBackground
        };
    }
    
    public static implicit operator MessageModalWindow.Message(Message m) => m.ToModalWindowMessage();
    public static implicit operator Message(MessageModalWindow.Message m) => new Message(m);
}
