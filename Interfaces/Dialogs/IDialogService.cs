
namespace Interfaces.Dialogs
{
    public interface IDialogService
    {
        void OpenDialog(IDialogViewModel DialogVm);
        void CloseDialog(IDialogViewModel dialogVM);
        void CloseAllDialogs();
        
    }
}
