using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileCamoes.View;
using MobileCamoes.ViewModel;
using MobileCamoes.ViewModel.Base;
using Xamarin.Forms;



namespace MobileCamoes.Services
{
    public class NavigationService : INavigationService
    {

        protected readonly Dictionary<Type, Type> _mappings;
        protected Application CurrentApplication
        {
            get => Application.Current;
        }

        public NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();
            CreateViewModelMappings();
        }

        private void CreateViewModelMappings()
        {
            _mappings.Add(typeof(MainViewModel), typeof(MainView));
            _mappings.Add(typeof(DetailViewModel), typeof(DetailVeiw));
        }

        public async Task Initialize()
        {
            await NavigateToAsync<MainViewModel>();
        }

        public async Task NavigateAndClearBackStackAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase
        {
            Page page = CreateAndBingPage(typeof(TViewModel), parameter);
            var navigationPage = CurrentApplication.MainPage as NavigationPage;

            await navigationPage.PushAsync(page);

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);

            if (navigationPage != null && navigationPage.Navigation.NavigationStack.Count > 0)
            {
                var existingPages = navigationPage.Navigation.NavigationStack.ToList();

                foreach (var item in existingPages)
                {
                    if (item != page)
                        navigationPage.Navigation.RemovePage(item);
                }
            }
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage != null)
            {
                var navigationPage = CurrentApplication.MainPage as NavigationPage;
                await navigationPage.PopAsync();
            }

        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
                    => InternalNavigateToAsync(typeof(TViewModel), null);

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
                    => InternalNavigateToAsync(typeof(TViewModel), parameter);

        public Task NavigateToAsync(Type viewModelType)
                    => InternalNavigateToAsync(viewModelType, null);

        public Task NavigateToAsync(Type viewModelType, object parameter)
                    => InternalNavigateToAsync(viewModelType, parameter);

        #region Not Implemented

        public Task RemoveLastFromBackStack()
        {
            throw new NotImplementedException();
        }

        #endregion

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBingPage(viewModelType, parameter);

            var navigationPage = CurrentApplication.MainPage as NavigationPage;

            if (navigationPage != null)
                await navigationPage.PushAsync(page);
            else
                CurrentApplication.MainPage = new NavigationPage(page);

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);

        }

        private Page CreateAndBingPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForVeiwModel(viewModelType);

            if (pageType == null)
                throw new Exception($"O mapeamento para o tipo {viewModelType} não existe");

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = ViewModelLocator.Instance.Resolve(viewModelType) as ViewModelBase;

            page.BindingContext = viewModel;
            return page;
        }

        private Type GetPageTypeForVeiwModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
                throw new Exception($"O tipo {viewModelType} não corresponde a nenhuma viewmodel");

            return _mappings[viewModelType];
        }
    }
}
