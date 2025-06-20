using JoeDevSharp.WinForms.Extensions.Reactive.Core;
using System;
using System.Collections;
using System.Windows.Forms;

namespace JoeDevSharp.WinForms.Extensions.Reactive
{
    public static class ControlBindingExtensions
    {
        private static void Bind<T>(this Control control, ReactiveProperty<T> property, ControlBindableProperty bindProperty)
        {
            string controlPropName = bindProperty.ToString();

            if (bindProperty == ControlBindableProperty.DataSource || bindProperty == ControlBindableProperty.Collection)
            {
                if (control is ListControl listControl && property.Value is IEnumerable enumerable)
                {
                    listControl.DataSource = null;
                    listControl.DataSource = enumerable;
                }
                else
                {
                    throw new InvalidOperationException($"El control '{control.GetType().Name}' no soporta DataSource con el tipo {typeof(T).Name}.");
                }
            }
            else
            {
                var existingBinding = control.DataBindings[controlPropName];
                if (existingBinding != null)
                    control.DataBindings.Remove(existingBinding);

                control.DataBindings.Add(controlPropName, property, nameof(property.Value), false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        #region Text Property Bindings

        public static void Bind<T>(this Button control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        public static void Bind<T>(this Label control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        public static void Bind<T>(this TextBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        public static void Bind<T>(this MaskedTextBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        public static void Bind<T>(this RichTextBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        public static void Bind<T>(this LinkLabel control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        #endregion

        #region Checked Property Bindings

        public static void Bind<T>(this CheckBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Checked);

        public static void Bind<T>(this RadioButton control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Checked);

        #endregion

        #region Value Property Bindings

        public static void Bind<T>(this NumericUpDown control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Value);

        public static void Bind<T>(this TrackBar control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Value);

        public static void Bind<T>(this ProgressBar control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Value);

        #endregion

        #region Image Property Bindings

        public static void Bind<T>(this PictureBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Image);

        #endregion

        #region DataSource / Collection Bindings

        public static void Bind<T>(this ComboBox comboBox, ReactiveProperty<IEnumerable> dataSource)
        {
            comboBox.DataSource = null;
            comboBox.DataSource = dataSource.Value;
            dataSource.PropertyChanged += (_, __) =>
            {
                comboBox.DataSource = null;
                comboBox.DataSource = dataSource.Value;
            };
        }

        public static void Bind<T>(this ListBox listBox, ReactiveProperty<IEnumerable> dataSource)
        {
            listBox.DataSource = null;
            listBox.DataSource = dataSource.Value;
            dataSource.PropertyChanged += (_, __) =>
            {
                listBox.DataSource = null;
                listBox.DataSource = dataSource.Value;
            };
        }
        public static void Bind<T>(this CheckedListBox checkedListBox, ReactiveCollention<T> items)
        {
            bool _isSyncing = false;

            void SyncSelectedToUI()
            {
                if (_isSyncing) return;
                _isSyncing = true;

                for (int i = 0; i < checkedListBox.Items.Count; i++)
                {
                    var item = (T)checkedListBox.Items[i];
                    bool shouldCheck = items.Selected.Contains(item);
                    if (checkedListBox.GetItemChecked(i) != shouldCheck)
                        checkedListBox.SetItemChecked(i, shouldCheck);
                }

                _isSyncing = false;
            }

            void SyncUIToSelected(ItemCheckEventArgs e)
            {
                if (_isSyncing) return;
                _isSyncing = true;

                var item = (T)checkedListBox.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    if (!items.Selected.Contains(item))
                        items.Selected.Add(item);
                }
                else
                {
                    if (items.Selected.Contains(item))
                        items.Selected.Remove(item);
                }

                items.Selected.NotifyUpdated();

                _isSyncing = false;
            }

            // Inicializa Items y sincroniza UI
            checkedListBox.Items.Clear();
            foreach (var item in items)
                checkedListBox.Items.Add(item);

            SyncSelectedToUI();

            // Sincroniza modelo -> UI
            items.Selected.PropertyChanged += (_, __) => SyncSelectedToUI();

            // Sincroniza UI -> modelo
            checkedListBox.ItemCheck += (_, e) => SyncUIToSelected(e);

            // Cuando cambia la lista de ítems
            items.PropertyChanged += (_, e) =>
            {
                if (_isSyncing) return;
                if (e.PropertyName != "Item[]") return;

                _isSyncing = true;
                checkedListBox.Items.Clear();
                foreach (var item in items)
                    checkedListBox.Items.Add(item);
                SyncSelectedToUI();
                _isSyncing = false;
            };
        }

        #endregion

        #region SelectedItem / SelectedIndex Bindings

        public static void Bind<T>(this ComboBox comboBox, ReactiveProperty<T> selectedItem)
        {
            bool syncing = false;

            void SyncModelToUI()
            {
                if (syncing) return;
                syncing = true;
                comboBox.SelectedItem = selectedItem.Value;
                syncing = false;
            }

            void SyncUIToModel(object? sender, EventArgs e)
            {
                if (syncing) return;
                syncing = true;
                if (comboBox.SelectedItem is T val)
                    selectedItem.Value = val;
                syncing = false;
            }

            selectedItem.PropertyChanged += (_, __) => SyncModelToUI();
            comboBox.SelectedIndexChanged += SyncUIToModel;
            SyncModelToUI();
        }

        public static void Bind<T>(this ListBox listBox, ReactiveCollention<T> selectedItems)
        {
            bool syncing = false;

            void SyncModelToUI()
            {
                if (syncing) return;
                syncing = true;

                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    var item = (T)listBox.Items[i];
                    listBox.SetSelected(i, selectedItems.Contains(item));
                }

                syncing = false;
            }

            void SyncUIToModel()
            {
                if (syncing) return;
                syncing = true;

                selectedItems.Clear();
                foreach (T selected in listBox.SelectedItems)
                    selectedItems.Add(selected);

                syncing = false;
            }

            listBox.SelectionMode = SelectionMode.One;
            listBox.SelectedIndexChanged += (s, e) => SyncUIToModel();
            selectedItems.ListChanged += (s, e) => SyncModelToUI();

            SyncModelToUI();
        }

        #endregion

        #region Special Controls Bindings

        public static void Bind(this DateTimePicker control, ReactiveProperty<DateTime> property)
        {
            bool syncing = false;

            void SyncModelToUI()
            {
                if (syncing) return;
                syncing = true;
                control.Value = property.Value;
                syncing = false;
            }

            void SyncUIToModel(object? sender, EventArgs e)
            {
                if (syncing) return;
                syncing = true;
                property.Value = control.Value;
                syncing = false;
            }

            property.PropertyChanged += (_, __) => SyncModelToUI();
            control.ValueChanged += SyncUIToModel;
            SyncModelToUI();
        }

        public static void Bind(this MonthCalendar control, ReactiveProperty<(DateTime Start, DateTime End)> property)
        {
            bool syncing = false;

            void SyncModelToUI()
            {
                if (syncing) return;
                syncing = true;
                control.SelectionStart = property.Value.Start;
                control.SelectionEnd = property.Value.End;
                syncing = false;
            }

            void SyncUIToModel(object? sender, EventArgs e)
            {
                if (syncing) return;
                syncing = true;
                property.Value = (control.SelectionStart, control.SelectionEnd);
                syncing = false;
            }

            property.PropertyChanged += (_, __) => SyncModelToUI();
            control.DateChanged += SyncUIToModel;
            SyncModelToUI();
        }

        public static void Bind<T>(this DomainUpDown control, ReactiveProperty<T> property)
        {
            bool syncing = false;

            void SyncModelToUI()
            {
                if (syncing) return;
                syncing = true;
                control.SelectedItem = property.Value;
                syncing = false;
            }

            void SyncUIToModel(object? sender, EventArgs e)
            {
                if (syncing) return;
                syncing = true;
                if (control.SelectedItem is T val)
                    property.Value = val;
                syncing = false;
            }

            property.PropertyChanged += (_, __) => SyncModelToUI();
            control.SelectedItemChanged += SyncUIToModel;
            SyncModelToUI();
        }

        public static void Bind(this TabControl control, ReactiveProperty<int> selectedIndex)
        {
            bool syncing = false;

            void SyncModelToUI()
            {
                if (syncing) return;
                syncing = true;
                control.SelectedIndex = selectedIndex.Value;
                syncing = false;
            }

            void SyncUIToModel(object? sender, EventArgs e)
            {
                if (syncing) return;
                syncing = true;
                selectedIndex.Value = control.SelectedIndex;
                syncing = false;
            }

            selectedIndex.PropertyChanged += (_, __) => SyncModelToUI();
            control.SelectedIndexChanged += SyncUIToModel;
            SyncModelToUI();
        }

        #endregion

        #region Complex controls without direct simple binding

        // DataGridView binding con BindingSource recomendado:
        public static void Bind<T>(this DataGridView control, ReactiveProperty<IEnumerable<T>> dataSource)
        {
            control.DataSource = null;
            control.DataSource = dataSource.Value;
            dataSource.PropertyChanged += (_, __) =>
            {
                control.DataSource = null;
                control.DataSource = dataSource.Value;
            };
        }

        #endregion
    }
}
