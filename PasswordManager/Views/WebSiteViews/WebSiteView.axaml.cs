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
    

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (DataContext != null)
        {
            var list = sender as ListBox;
            ((WebSiteViewModel)DataContext!).CurrentItem = (WebSiteItemViewModel)list?.SelectedItem!;
        }
    }
    

}