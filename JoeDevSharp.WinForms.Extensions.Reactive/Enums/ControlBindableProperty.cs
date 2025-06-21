namespace JoeDevSharp.WinForms.Extensions.Reactive
{
    /// <summary>
    /// Defines the set of bindable properties for Windows Forms controls.
    /// Used to indicate which property of the control should be bound to a reactive property.
    /// </summary>
    public enum ControlBindableProperty
    {
        /// <summary>
        /// Binds to the Text property of a control.
        /// </summary>
        Text,

        /// <summary>
        /// Binds to the Value property of a control.
        /// </summary>
        Value,

        /// <summary>
        /// Binds to the Image property of a control.
        /// </summary>
        Image,

        /// <summary>
        /// Binds to the SelectedItem property of a control.
        /// </summary>
        SelectedItem,

        /// <summary>
        /// Binds to the Checked property of a control.
        /// </summary>
        Checked,

        /// <summary>
        /// Binds to the DataSource property of a control.
        /// </summary>
        DataSource,

        /// <summary>
        /// Binds to a generic collection used by a control.
        /// </summary>
        Collection,

        /// <summary>
        /// Binds to the SelectedIndex property of a control.
        /// </summary>
        SelectedIndex
    }
}
