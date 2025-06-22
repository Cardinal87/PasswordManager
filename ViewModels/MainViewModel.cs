
using ViewModels.WebSiteViewModels;
using ViewModels.AllEntriesViewModels;
using ViewModels.AppViewModels;
using CommunityToolkit.Mvvm.Input;
using ViewModels.CardViewModels;
using ViewModels.BaseClasses;


namespace ViewModels;

public partial class MainViewModel : ViewModelBase
{

    private ViewModelBase? currentPage;
    

    public ViewModelBase? CurrentPage {

        get { return currentPage; }
        set {
            currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    public MainViewModel(AppViewModel appViewModel,
        CardViewModel cardViewModel,
        WebSiteViewModel webSiteViewModel)
    {
        SetCurrentPageCommand = new RelayCommand<ViewModelBase>(SetCurrentPage);
        AppVm = appViewModel;
        WebSiteVm = webSiteViewModel;
        CardVm = cardViewModel;
    }


    public RelayCommand<ViewModelBase> SetCurrentPageCommand { get; set; }
    public AllEntriesViewModel? AllEntriesVm { get; private set; }
    public AppViewModel AppVm { get; private set; }
    public WebSiteViewModel WebSiteVm { get; private set; }
    public CardViewModel CardVm { get; private set; }
    
    private void SetCurrentPage(ViewModelBase? vm)
    {
        CurrentPage = vm;
    }

    public async Task InizializeViewModelsAsync()
    {
        var webTask = WebSiteVm.LoadViewModelsListAsync();
        var cardTask = CardVm.LoadViewModelsListAsync();
        var appTask = AppVm.LoadViewModelListAsync();

        await webTask;
        await cardTask;
        await appTask;

        AllEntriesVm = new AllEntriesViewModel(AppVm, CardVm, WebSiteVm);
        CurrentPage = WebSiteVm;
    }

    public void ResetData()
    {
        AppVm.Apps.Clear();
        WebSiteVm.WebSites.Clear();
        CardVm.Cards.Clear();
        AllEntriesVm?.Items.Clear();
    }

}
