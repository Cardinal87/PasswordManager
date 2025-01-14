using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Diagnostics;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.Interfaces;
using PasswordManager.ViewModels.WebSiteViewModels;

namespace PasswordManager.Views.WebSiteViews;

public partial class WebSiteDialogView : Window
{
    public WebSiteDialogView()
    {
        InitializeComponent();
        this.AttachDevTools();
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
        NameBox.Text = string.Empty;
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.Default;
        MainBorder.Focus();
    }

    private void ConfirmName(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        if (NameBox.Text != null)
        {
            var vm = (WebSiteDialogViewModel)DataContext!;
            vm.Name = NameBox.Text;
            CloseTemplate(sender, e);
        }
        
        
    }

    private void QuickConfirmName(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ConfirmName(sender, e);
            e.Handled = true;
            MainBorder.Focus();
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