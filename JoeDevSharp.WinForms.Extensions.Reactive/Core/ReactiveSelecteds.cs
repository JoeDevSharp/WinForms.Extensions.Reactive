using System.ComponentModel;

namespace JoeDevSharp.WinForms.Extensions.Reactive.Core
{
    public class ReactiveSelected<T> : BindingList<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ReactiveSelected() : base()
        {
            // Escuchar cambios internos de la lista para propagar PropertyChanged
            this.ListChanged += OnListChangedInternal;
        }

        private void OnListChangedInternal(object? sender, ListChangedEventArgs e)
        {
            // Notificar cambio de la colección para los bindings
            NotifyPropertyChanged(nameof(Count));
            NotifyPropertyChanged("Item[]");
        }

        /// <summary>
        /// Método para notificar cambios manuales (si modificas elementos fuera del BindingList)
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
