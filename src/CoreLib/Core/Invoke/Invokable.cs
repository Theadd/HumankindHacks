using System;
using System.Linq.Expressions;

namespace AnN3x.CoreLib;

public class Invokable<T>
{
    private static readonly Func<T> DefaultAction = () => default(T);
    private Func<T> _action = DefaultAction;
    private T _value;

    public bool IsInvokable { get; private set; }
    
    public T Value
    {
        get => IsInvokable ? _action.Invoke() : _value;
        set
        {
            _value = value;
            IsInvokable = false;
        }
    }
    
    public Func<T> Action
    {
        get => _action;
        set
        {
            IsInvokable = true;
            _action = value ?? DefaultAction;
        }
    }
    
    public Invokable(T value) => Value = value;
    public Invokable(Func<T> action) => Action = action;
    // public Invokable(Function<T> action) => Action = action.CastDelegate<Func<T>>();
    public Invokable(Invokable<T> other)
    {
        IsInvokable = other.IsInvokable;
        _value = other._value;
        _action = other._action;
    }

    public static implicit operator T(Invokable<T> ins) => ins.Value;
    public static implicit operator Func<T>(Invokable<T> m) => m.Action;
    public static implicit operator Invokable<T>(T val) => new Invokable<T>(val);
    public static implicit operator Invokable<T>(Func<T> fn) => new Invokable<T>(fn);
    // public static implicit operator Invokable<T>(Function<T> fn) => new Invokable<T>(fn);
    public static implicit operator Invokable<T>(Expression<Func<T>> e) => new Invokable<T>((Func<T>)e.Compile());
}
