using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Views;

public partial class PasswordGeneratorView : Window
{
    public PasswordGeneratorView()
    {
        InitializeComponent();
    }
    private void MoveWindow(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (!MainBorder.IsFocused)
            MainBorder.Focus();
        BeginMoveDrag(e);

    }


}