using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace PasswordManager.Views.AppViews;

public partial class AppView : UserControl
{
    public AppView()
    {
        InitializeComponent();
    }
    public void EnterPressed(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {

        }
    }
}