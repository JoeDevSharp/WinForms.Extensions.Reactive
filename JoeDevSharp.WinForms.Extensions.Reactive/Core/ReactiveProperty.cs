using System.ComponentModel;
namespace JoeDevSharp.WinForms.Extensions.Reactive.Core;
public class ReactiveProperty<T> : INotifyPropertyChanged
{
    private T _value;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ReactiveProperty(T initialValue = default)
    {
        _value = initialValue;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }
    public void SetValue(T value)
    {
        if (!Equals(_value, value))
        {
            _value = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
    public override string ToString() => _value?.ToString();
}