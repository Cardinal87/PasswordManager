using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.CardViewModels;

namespace PasswordManager.Views.CardViews;

public partial class CardDialogView : Window
{
    public CardDialogView()
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
        ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.Default;
        MainBorder.Focus();
    }

    private void ConfirmName(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(NameBox.Text))
        {
            var vm = (CardDialogViewModel)DataContext!;
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

    private void QuickConfirmName(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ConfirmName(sender, e);
            e.Handled = true;
        }
    }

    private void ConfirmData(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            MainBorder.Focus();
        }
    }
    

    private void Digit_Validation(object? sender, KeyEventArgs e)
    {
        ConfirmData(sender, e);
        if (!e.Handled && !char.IsDigit(e.KeySymbol![0])) e.Handled = true;
        
    }
}