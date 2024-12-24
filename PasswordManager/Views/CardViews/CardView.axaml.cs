using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;

namespace PasswordManager.Views.CardViews;

public partial class CardView : UserControl
{
    public CardView()
    {
        InitializeComponent();
    }

    public void EnterPressed(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var text = ((TextBox)sender).Text;
            var vm = (CardViewModel)DataContext!;
            if (text != null) vm.SearchKey = text;
        }
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (DataContext != null)
        {
            var list = sender as ListBox;
            ((CardViewModel)DataContext!).CurrentItem = (CardItemViewModel)list?.SelectedItem!;
        }
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