using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views.AppViews;

public partial class AppItemView : UserControl
{
    public AppItemView()
    {
        InitializeComponent();
    }
    private void ChangePasswordVisibility(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var button = (Button)sender!;
        button.Tag = !(bool)button.Tag!;
        PasswordGrid.Tag = !(bool)PasswordGrid.Tag!;


    }
    private void TextCopied(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TextCopiedMessage.Tag = false;
        TextCopiedMessage.Tag = true;
    }
}