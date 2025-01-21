using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Views;

public partial class MenuView : UserControl
{
    public MenuView()
    {
        InitializeComponent();
    }

    private void ShowConfirmationWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoggingGrid.IsVisible = false;
        ConfirmationGrid.IsVisible = true;
    }
    private void HideConfirmationWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        LoggingGrid.IsVisible = true;
        ConfirmationGrid.IsVisible = false;
    }

    private void MenuItem_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}