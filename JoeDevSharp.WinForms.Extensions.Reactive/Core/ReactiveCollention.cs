using JoeDevSharp.WinForms.Extensions.Reactive.Core;
using System.ComponentModel;

namespace JoeDevSharp.WinForms.Extensions.Reactive
{
    public class ReactiveCollention<T> : BindingList<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ReactiveSelected<T> Selected = new ReactiveSelected<T>();
        public ReactiveProperty<T> SelectedItem = new ReactiveProperty<T>();

        public ReactiveCollention() : base() { }
        public ReactiveCollention(BindingList<T> list) : base(list) { }

        public void NotifyUpdated()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            base.OnListChanged(e);
            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged("Item[]"); // Notifica cambios en los elementos
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}