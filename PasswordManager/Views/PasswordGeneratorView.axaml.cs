using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views;

public partial class PasswordGeneratorView : Window
{
    public PasswordGeneratorView()
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


}