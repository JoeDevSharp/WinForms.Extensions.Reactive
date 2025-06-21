# WinForms Reactive Extension

`JoeDevSharp.WinForms.Extensions.Reactive` provides a simple yet powerful way to introduce reactive data binding into Windows Forms applications. Unlike traditional WinForms development where UI state management requires manual wiring of events and property updates, this framework automates synchronization between the UI and data models using reactive properties.

Its API is designed to be minimal and intuitive: with a single extension method call, any compatible control can bind to reactive properties, dramatically reducing boilerplate code and improving maintainability.

By leveraging this reactive approach, developers gain:

* Real-time, two-way synchronization between UI elements and underlying data.
* Clean separation between UI and business logic layers.
* Enhanced support for dynamic collections and selection states.
* Reduced risk of inconsistent UI state and bugs related to manual updates.

This framework fits naturally into existing WinForms projects and offers a scalable foundation for building complex, responsive desktop applications.

## Introduction

`JoeDevSharp.WinForms.Extensions.Reactive` is a lightweight reactive binding framework for Windows Forms applications. Inspired by modern reactive frameworks like Vue.js, it enables automatic two-way synchronization between UI control properties and reactive data models (`ReactiveProperty<T>`, `ReactiveCollention<T>`).

Its goal is to simplify state management in traditional WinForms interfaces, making applications cleaner, decoupled, and easier to maintain — without the boilerplate of manual event handling and UI updates.

With this library, you can:

* Bind control properties such as text, value, checked state, selection, and collections directly to reactive models.
* Manage dynamic lists and multiple selections with automatic notifications.
* Reduce repetitive code by automating UI-to-model synchronization.

This document is aimed at developers who want to integrate or extend this framework in their WinForms projects, providing a clear guide to its core concepts, main classes, and usage patterns.

---

## Key Concepts

### ReactiveProperty<T>

A reactive property that wraps a value and automatically notifies listeners of changes.
Ideal for simple values (string, bool, int, etc.) that need to stay in sync with UI controls.

```csharp
var name = new ReactiveProperty<string>("initial");
name.PropertyChanged += (s, e) => { /* react to change */ };
name.Value = "new value"; // triggers PropertyChanged event
```

### ReactiveCollention<T>

A reactive collection extending `BindingList<T>`, notifying about changes in items and selection state.
Perfect for dynamic lists bound to controls like `ListBox` or `CheckedListBox`.

### ReactiveSelected<T>

Tracks and notifies the selection state within a reactive collection.
Useful for bindings with controls that allow multiple selected items.

### ControlBindableProperty

An enum defining which WinForms control properties can be bound reactively, such as `Text`, `Checked`, `Value`, etc.

---

## Quick Start

### 1. Create Your ViewModel with Reactive Properties

Define your data model using `ReactiveProperty<T>` and `ReactiveCollention<T>` to represent values and collections reactively.

```csharp
public class MyViewModel
{
    public ReactiveProperty<string> UserName { get; } = new();
    public ReactiveProperty<bool> IsAdmin { get; } = new();
    public ReactiveCollention<string> Roles { get; } = new();
    public ReactiveProperty<string> SelectedRole { get; } = new();
}
```

### 2. Bind Controls to ViewModel Properties

In your form, bind WinForms controls to these reactive properties with the provided extension methods.

```csharp
public partial class MainForm : Form
{
    private MyViewModel _viewModel = new();

    public MainForm()
    {
        InitializeComponent();

        // Bind TextBox.Text to UserName
        textBoxUserName.Bind(_viewModel.UserName);

        // Bind CheckBox.Checked to IsAdmin
        checkBoxIsAdmin.Bind(_viewModel.IsAdmin);

        // Bind ComboBox DataSource to Roles collection
        comboBoxRoles.Bind(_viewModel.Roles);

        // Bind ComboBox SelectedItem to SelectedRole
        comboBoxRoles.Bind(_viewModel.SelectedRole);
    }
}
```

### 3. Update ViewModel and Watch UI Update Automatically

Any change to `_viewModel.UserName.Value`, `_viewModel.IsAdmin.Value`, or the collections will immediately reflect in the UI without manual event handling.

```csharp
_viewModel.UserName.Value = "newuser";
_viewModel.IsAdmin.Value = true;
_viewModel.Roles.Add("Admin");
```
