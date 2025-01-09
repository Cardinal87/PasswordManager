using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;

namespace PasswordManager.Views.CardViews;

public partial class CardView : UserControl
{
    public CardView()
    {
        InitializeComponent();
    }

    private void SearchBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            var text = textBox.Text;
            var vm = (AppViewModel)DataContext!;
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
    
}