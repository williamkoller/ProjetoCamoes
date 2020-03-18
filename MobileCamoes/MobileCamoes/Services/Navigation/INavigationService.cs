using System;
using System.Threading.Tasks;
using MobileCamoes.ViewModel.Base;

namespace MobileCamoes.Services
{
    public interface INavigationService
    {
        Task Initialize();
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToAsync(Type viewModelType);
        Task NavigateToAsync(Type viewModelType, object parameter);
        Task NavigateBackAsync();
        Task NavigateAndClearBackStackAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase;
        Task RemoveLastFromBackStack();
    }
}
