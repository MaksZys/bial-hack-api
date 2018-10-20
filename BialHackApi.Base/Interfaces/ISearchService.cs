using BialHackApi.Base.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BialHackApi.Base.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<string>> Suggest(string query);
        Task<IEnumerable<TrashTransportDTO>> Search(string query);
    }
}
