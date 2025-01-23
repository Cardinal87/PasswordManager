using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

using ViewModels.AppViewModels;
using ViewModels.CardViewModels;

namespace Views.CardViews;

public partial class CardDialogView : Window
{
    public CardDialogView()
    {
        InitializeComponent();
        prevDate = DataContext is CardDialogViewModel vm ? vm.Date : "";
    }
    private string prevDate;

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

    private void QuickConfirmName(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            ConfirmName(sender, e);
            e.Handled = true;
            MainBorder.Focus();
        }
    }
    private void ConfirmName(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (NameBox.Text != null)
        {
            var vm = (CardDialogViewModel)DataContext!;
            vm.Name = NameBox.Text;
            CloseTemplate(sender, e);
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
        if (!e.Handled && e.KeySymbol != null && !char.IsDigit(e.KeySymbol[0])) e.Handled = true;
        
    }

    private void DataBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (sender is TextBox t && t.Text != null) {
            if (t.Text.Length == 3 && prevDate.Length < t.Text.Length)
            {
                t.Text = t.Text.Insert(2, "/");
                t.CaretIndex = 4;
            }
            int ind = t.CaretIndex;
            t.CaretIndex = 0;
            t.CaretIndex = ind;
            prevDate = t.Text;
        }
    }
}