using JoeDevSharp.WinForms.Extensions.Reactive.Core;
using System;
using System.Collections;
using System.Windows.Forms;

namespace JoeDevSharp.WinForms.Extensions.Reactive
{
    /// <summary>
    /// Provides extension methods to bind WinForms controls to reactive properties.
    /// </summary>
    public static class ControlBindingExtensions
    {
        /// <summary>
        /// Performs a generic binding between a control property and a reactive property.
        /// Supports both standard bindings and specific logic for DataSource/Collection.
        /// </summary>
        /// <typeparam name="T">The type of the bound property.</typeparam>
        /// <param name="control">The control to bind.</param>
        /// <param name="property">The reactive property to bind to.</param>
        /// <param name="bindProperty">The target control property to bind.</param>
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
                    throw new InvalidOperationException($"Control '{control.GetType().Name}' does not support DataSource with type {typeof(T).Name}.");
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

        /// <summary>
        /// Binds the Text property of a Button control to a reactive property.
        /// </summary>
        public static void Bind<T>(this Button control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        /// <summary>
        /// Binds the Text property of a Label control to a reactive property.
        /// </summary>
        public static void Bind<T>(this Label control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        /// <summary>
        /// Binds the Text property of a TextBox control to a reactive property.
        /// </summary>
        public static void Bind<T>(this TextBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        /// <summary>
        /// Binds the Text property of a MaskedTextBox control to a reactive property.
        /// </summary>
        public static void Bind<T>(this MaskedTextBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        /// <summary>
        /// Binds the Text property of a RichTextBox control to a reactive property.
        /// </summary>
        public static void Bind<T>(this RichTextBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        /// <summary>
        /// Binds the Text property of a LinkLabel control to a reactive property.
        /// </summary>
        public static void Bind<T>(this LinkLabel control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Text);

        #endregion

        #region Checked Property Bindings

        /// <summary>
        /// Binds the Checked property of a CheckBox control to a reactive property.
        /// </summary>
        public static void Bind<T>(this CheckBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Checked);

        /// <summary>
        /// Binds the Checked property of a RadioButton control to a reactive property.
        /// </summary>
        public static void Bind<T>(this RadioButton control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Checked);

        #endregion

        #region Value Property Bindings

        /// <summary>
        /// Binds the Value property of a NumericUpDown control to a reactive property.
        /// </summary>
        public static void Bind<T>(this NumericUpDown control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Value);

        /// <summary>
        /// Binds the Value property of a TrackBar control to a reactive property.
        /// </summary>
        public static void Bind<T>(this TrackBar control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Value);

        /// <summary>
        /// Binds the Value property of a ProgressBar control to a reactive property.
        /// </summary>
        public static void Bind<T>(this ProgressBar control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Value);

        #endregion

        #region Image Property Bindings

        /// <summary>
        /// Binds the Image property of a PictureBox control to a reactive property.
        /// </summary>
        public static void Bind<T>(this PictureBox control, ReactiveProperty<T> property)
            => Bind(control, property, ControlBindableProperty.Image);

        #endregion

        #region DataSource / Collection Bindings

        /// <summary>
        /// Binds a ComboBox control to a reactive enumerable data source.
        /// </summary>
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

        /// <summary>
        /// Binds a ListBox control to a reactive enumerable data source.
        /// </summary>
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

        /// <summary>
        /// Binds a CheckedListBox control to a reactive collection with selection state tracking.
        /// </summary>
        public static void Bind<T>(this CheckedListBox checkedListBox, ReactiveCollention<T> items)
        {
            // Implementation omitted for brevity.
        }

        #endregion

        #region SelectedItem / SelectedIndex Bindings

        /// <summary>
        /// Binds the SelectedItem of a ComboBox to a reactive property.
        /// </summary>
        public static void Bind<T>(this ComboBox comboBox, ReactiveProperty<T> selectedItem)
        {
            // Implementation omitted for brevity.
        }

        /// <summary>
        /// Binds the selected items of a ListBox to a reactive collection.
        /// </summary>
        public static void Bind<T>(this ListBox listBox, ReactiveCollention<T> selectedItems)
        {
            // Implementation omitted for brevity.
        }

        #endregion

        #region Special Controls Bindings

        /// <summary>
        /// Binds a DateTimePicker control to a reactive DateTime property.
        /// </summary>
        public static void Bind(this DateTimePicker control, ReactiveProperty<DateTime> property)
        {
            // Implementation omitted for brevity.
        }

        /// <summary>
        /// Binds a MonthCalendar control to a reactive date range.
        /// </summary>
        public static void Bind(this MonthCalendar control, ReactiveProperty<(DateTime Start, DateTime End)> property)
        {
            // Implementation omitted for brevity.
        }

        /// <summary>
        /// Binds a DomainUpDown control to a reactive property.
        /// </summary>
        public static void Bind<T>(this DomainUpDown control, ReactiveProperty<T> property)
        {
            // Implementation omitted for brevity.
        }

        /// <summary>
        /// Binds the SelectedIndex of a TabControl to a reactive integer property.
        /// </summary>
        public static void Bind(this TabControl control, ReactiveProperty<int> selectedIndex)
        {
            // Implementation omitted for brevity.
        }

        #endregion

        #region Complex controls without direct simple binding

        /// <summary>
        /// Binds a DataGridView to a reactive enumerable property. BindingSource is recommended for advanced scenarios.
        /// </summary>
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
