using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views.CardViews;

public partial class CardItemView : UserControl
{
    public CardItemView()
    {
        InitializeComponent();
    }

    private void ChangePasswordVisibility(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var button = (Button)sender!;
        button.Tag = !(bool)button.Tag!;
        CvcGrid.Tag = !(bool)CvcGrid.Tag!;


    }
    private void TextCopied(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TextCopiedMessage.Tag = false;
        TextCopiedMessage.Tag = true;
    }
}