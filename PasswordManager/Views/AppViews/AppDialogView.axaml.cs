using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.AppViewModels;
using System;

namespace PasswordManager.Views.AppViews;

public partial class AppDialogView : Window
{
    public AppDialogView()
    {
        InitializeComponent();
    }
    private void MoveWindow(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }

    private void ShowSetNameTemplate(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowName.IsVisible = false;
        SetName.IsVisible = true;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
        SetName.Focus();
    }

    private void CloseTemplate(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowName.IsVisible = true;
        SetName.IsVisible = false;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.Default;
    }

    private void ConfirmName(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(NameBox.Text))
        {
            var vm = (AppDialogViewModel)DataContext!;
            vm.Name = NameBox.Text;
            CloseTemplate(sender, e);
        }
        else
        {
            ConfirmNameButton.IsEnabled = false;
            NameWarning.IsVisible = true;
        }
    }
    private void NameChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(NameBox.Text))
        {
            ConfirmNameButton.IsEnabled = true;
            NameWarning.IsVisible = false;
        }
    }
}