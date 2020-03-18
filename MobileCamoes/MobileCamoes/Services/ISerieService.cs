using MobileCamoes.Model;
using System.Threading.Tasks;

namespace MobileCamoes.Services
{
    public interface ISerieService
    {
        Task<SerieResponse> GetSeriesAsync();
    }
}
