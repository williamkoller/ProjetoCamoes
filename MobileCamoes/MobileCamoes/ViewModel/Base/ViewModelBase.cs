using System.Threading.Tasks;
using MobileCamoes.Services;
using Xamarin.Forms;

namespace MobileCamoes.ViewModel.Base
{
    public abstract class ViewModelBase : BindableObject
    {
        protected readonly INavigationService NavigationService;

        string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public ViewModelBase(string title)
        {
            Title = title;
            NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
        }


        public virtual Task InitializeAsync(object navgationData)
        {
            return Task.FromResult(true);
        }
    }
}
