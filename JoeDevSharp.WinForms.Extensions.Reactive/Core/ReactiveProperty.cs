using System.ComponentModel;

namespace JoeDevSharp.WinForms.Extensions.Reactive.Core
{
    /// <summary>
    /// Represents a reactive property that notifies listeners when its value changes.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class ReactiveProperty<T> : INotifyPropertyChanged
    {
        private T _value;

        /// <summary>
        /// Occurs when the value of the property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveProperty{T}"/> class with an optional initial value.
        /// </summary>
        /// <param name="initialValue">The initial value of the property.</param>
        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }

        /// <summary>
        /// Gets or sets the value of the property. Triggers PropertyChanged when the value changes.
        /// </summary>
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

        /// <summary>
        /// Sets the value of the property explicitly. Triggers PropertyChanged if the value is different.
        /// </summary>
        /// <param name="value">The new value to assign.</param>
        public void SetValue(T value)
        {
            if (!Equals(_value, value))
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        /// <summary>
        /// Returns the string representation of the underlying value.
        /// </summary>
        /// <returns>The result of calling ToString() on the value.</returns>
        public override string ToString() => _value?.ToString();
    }
}
