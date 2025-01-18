using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ViewModels.AllEntriesViewModels;
using ViewModels.AppViewModels;
using ViewModels.BaseClasses;


namespace Views.AllEntriesViews;

public partial class AllEntriesView : UserControl
{
    public AllEntriesView()
    {
        InitializeComponent();
        
    }
    public void EnterPressed(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var text = ((TextBox)sender).Text;
            var vm = (AppViewModel)DataContext!;
            if (text != null) vm.SearchKey = text;
        }
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (DataContext != null)
        {
            var list = sender as ListBox;
            ((AllEntriesViewModel)DataContext!).CurrentItem = (ItemViewModelBase)list?.SelectedItem!;
        }
    }

    private void CardItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ((AllEntriesViewModel)DataContext!).CardViewModel.AddNewCommand.Execute(null);
    }

    
}