using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views.WebSiteViews;

public partial class WebSiteItemView : UserControl
{
    public WebSiteItemView()
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