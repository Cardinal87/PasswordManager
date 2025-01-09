using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.AppViewModels;
using System;

namespace PasswordManager.Views.AppViews;

public partial class AppView : UserControl
{
    public AppView()
    {
        InitializeComponent();
    }
    

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (DataContext != null)
        {
            var list = sender as ListBox;
            ((AppViewModel)DataContext!).CurrentItem = (AppItemViewModel)list?.SelectedItem!;
        }
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
}