using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.Interfaces;
using PasswordManager.ViewModels.WebSiteViewModels;

namespace PasswordManager.Views.WebSiteViews;

public partial class WebSiteDialogView : Window
{
    public WebSiteDialogView()
    {
        InitializeComponent();
    }
    private void MoveWindow(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (!MainBorder.IsFocused)
            MainBorder.Focus();
        BeginMoveDrag(e);
    }

    private void ShowSetNameTemplate(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowName.IsVisible = false;
        SetName.IsVisible = true;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
        NameBox.Focus();
    }

    private void CloseTemplate(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ShowName.IsVisible = true;
        SetName.IsVisible = false;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.Default;
        MainBorder.Focus();
    }

    private void ConfirmName(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(NameBox.Text))
        {
            var vm = (WebSiteDialogViewModel)DataContext!;
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

    private void QuickConfirmName(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ConfirmName(sender, e);
            e.Handled = true;
        }
    }

    private void ConfirmData(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            MainBorder.Focus();
        }
    }
    


}