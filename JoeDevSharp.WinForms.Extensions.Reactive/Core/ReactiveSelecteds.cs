using System.ComponentModel;

namespace JoeDevSharp.WinForms.Extensions.Reactive.Core
{
    /// <summary>
    /// Represents a reactive list of selected items that notifies listeners of changes.
    /// Useful for UI bindings that track multiple selections.
    /// </summary>
    /// <typeparam name="T">The type of selected items.</typeparam>
    public class ReactiveSelected<T> : BindingList<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveSelected{T}"/> class.
        /// </summary>
        public ReactiveSelected() : base()
        {
            // Listen for internal list changes to trigger PropertyChanged
            this.ListChanged += OnListChangedInternal;
        }

        private void OnListChangedInternal(object? sender, ListChangedEventArgs e)
        {
            // Notify binding systems about changes in Count and Item[]
            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged("Item[]");
        }

        /// <summary>
        /// Manually notifies that the item collection has changed.
        /// Useful when modifying items outside of BindingList operations.
        /// </summary>
        public void NotifyUpdated()
        {
            NotifyPropertyChanged("Item[]");
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
