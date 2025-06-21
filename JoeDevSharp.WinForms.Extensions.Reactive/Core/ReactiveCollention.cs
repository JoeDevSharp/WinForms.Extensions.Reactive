using JoeDevSharp.WinForms.Extensions.Reactive.Core;
using System.ComponentModel;

namespace JoeDevSharp.WinForms.Extensions.Reactive
{
    /// <summary>
    /// Represents a reactive collection that supports property change notifications and selected item tracking.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class ReactiveCollention<T> : BindingList<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Represents the selected items within the collection.
        /// </summary>
        public ReactiveSelected<T> Selected = new ReactiveSelected<T>();

        /// <summary>
        /// Represents the currently selected item in the collection.
        /// </summary>
        public ReactiveProperty<T> SelectedItem = new ReactiveProperty<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveCollention{T}"/> class.
        /// </summary>
        public ReactiveCollention() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveCollention{T}"/> class from an existing BindingList.
        /// </summary>
        /// <param name="list">The source list to initialize from.</param>
        public ReactiveCollention(BindingList<T> list) : base(list) { }

        /// <summary>
        /// Notifies listeners that the collection items have been updated.
        /// Triggers a PropertyChanged event for \"Item[]\".
        /// </summary>
        public void NotifyUpdated()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }

        /// <summary>
        /// Overrides the base method to raise additional property change notifications when the list changes.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnListChanged(ListChangedEventArgs e)
        {
            base.OnListChanged(e);
            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged("Item[]");
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
