using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;

namespace PasswordManager.Views.WebSiteViews;

public partial class WebSiteView : UserControl
{
    public WebSiteView()
    {
        InitializeComponent();
    }
    public void EnterPressed(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var text = ((TextBox)sender).Text;
            var vm = (WebSiteViewModel)DataContext!;
            if (text != null) vm.SearchKey = text;
        }
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (DataContext != null)
        {
            var list = sender as ListBox;
            ((WebSiteViewModel)DataContext!).CurrentItem = (WebSiteItemViewModel)list?.SelectedItem!;
        }
    }
    

}