using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatApp.WindowsClient.Services.Interfaces
{
    public interface INavigationService
    {
        ObservableObject CurrentView { get; }
        void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
    }
}
