using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PasswordManager.ViewModels.DialogInterfaces;
using PasswordManager.ViewModels.WebSiteViewModels;

namespace PasswordManager.Views.WebSiteViews;

public partial class WebSiteDialog : Window
{
    public WebSiteDialog()
    {
        InitializeComponent();
        (this.DataContext as WebSiteDialogViewModel)!.dialogResultRequest += CloseWindow!;
    }
    private void CloseWindow(object sender, DialogResultEventArgs e) => this.Close();


}