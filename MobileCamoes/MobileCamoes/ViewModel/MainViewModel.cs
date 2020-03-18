using MobileCamoes.Model;
using MobileCamoes.Services;
using MobileCamoes.ViewModel.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCamoes.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        readonly ISerieService _serieService;

        public ICommand ItemClickCommand { get; }


        public ObservableCollection<Serie> Items { get; }

        public MainViewModel(ISerieService serieService) : base("Camões - Mobile - 2020")
        {
            _serieService = serieService;

            Items = new ObservableCollection<Serie>();

            ItemClickCommand = new Command<Serie>(async (item) => await ItemClickCommandExecute(item));
        }

        private async Task ItemClickCommandExecute(Serie item)
        {
            if (item != null)
            {
                await NavigationService.NavigateToAsync<DetailViewModel>(item);
            }
        }

        public override async Task InitializeAsync(object navgationData)
        {
            await base.InitializeAsync(navgationData);
            await LoadDataAsync();
        }

        async Task LoadDataAsync()
        {
            var result = await _serieService.GetSeriesAsync();

            AddItens(result);
        }

        private void AddItens(SerieResponse result)
        {
            Items.Clear();
            result?.Series.ToList()?.ForEach(i => Items.Add(i));
        }
    }
}
