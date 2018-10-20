using BialHackApi.Base.DTO;
using BialHackApi.Base.Interfaces;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BialHackApi.Base.Services
{
    public class SearchService : ISearchService
    {
        protected readonly SearchServiceClient searchClient;
        protected readonly ISearchIndexClient indexClient;

        public SearchService()
        {
            searchClient = new SearchServiceClient("hack-service", new SearchCredentials("11DFA80063068E8F56C74494078A7012"));
            indexClient = searchClient.Indexes.GetClient("azuresql-index");
        }

        public async Task<IEnumerable<TrashTransportDTO>> Search(string query)
        {
            DocumentSearchResult response;

            try
            {
                response = await SearchDocument(query);
            }
            catch (CloudException ex)
            {
                response = new DocumentSearchResult();
            }

            return await DeserializeResults(response);
        }

        private async Task<IEnumerable<TrashTransportDTO>> DeserializeResults(DocumentSearchResult response)
        {
            var results = new List<TrashTransportDTO>();

            foreach (var result in response.Results)
            {
                var date = result.Document["Date"] != null ? DateTime.Parse(result.Document["Date"].ToString()) : DateTime.MinValue;

                results.Add(new TrashTransportDTO
                {
                    Container = result.Document["Container"] != null ? result.Document["Container"].ToString() : "",
                    Note = result.Document["Note"] != null ? result.Document["Note"].ToString() : "",
                    Description = result.Document["Description"] != null ? result.Document["Description"].ToString() : "",
                    RfId0 = result.Document["RfId0"] != null ? result.Document["RfId0"].ToString() : "",
                    VehicleName = result.Document["VehicleName"] != null ? result.Document["VehicleName"].ToString() : "",
                    MgoType = result.Document["MgoType"] != null ? result.Document["MgoType"].ToString() : "",
                    Latitude = result.Document["Latitude"] != null ? Convert.ToDouble(result.Document["Latitude"].ToString()) : 0,
                    Longitude = result.Document["Longitude"] != null ? Convert.ToDouble(result.Document["Longitude"].ToString()) : 0,
                    TrashType = result.Document["TrashType"] != null ? result.Document["TrashType"].ToString() : "",
                    VehicleNumber = result.Document["VehicleNumber"] != null ? result.Document["VehicleNumber"].ToString() : "",
                    Date = date
                });
            }
            return results;
        }

        public async Task<DocumentSearchResult> SearchDocument(string query)
        {
            var searchParameters = new SearchParameters()
            {
                QueryType = QueryType.Full
            };

            return indexClient.Documents.Search(query, searchParameters);
        }


        public async Task<IEnumerable<string>> Suggest(string query)
        {
            var suggestParameters = new SuggestParameters()
            {
                UseFuzzyMatching = true,
                Top = 15,
                OrderBy = new[] { "search.score() desc" }
            };

             var documents = indexClient.Documents.Suggest(query, "suggester", suggestParameters);

            var suggestions = new List<string>();
            foreach (var result in documents.Results)
            {
                suggestions.Add(result.Document["TextTitle"].ToString());
            }

            return suggestions.Distinct().Take(10).ToList();
        }
    }
}
