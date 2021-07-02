using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Properties = ElastiRestApi.Business.JsonObjects.Properties;

namespace ElastiRestApi.API
{
    public class ElastiApiMethods : IElastiApiMethods
    {
        private readonly ElasticClient _client;

        public ElastiApiMethods(ElasticClient client) =>
            _client = client;

        public Task<ISearchResponse<Properties>> SearchTask(string query, string marketList)
        {
            return _client.SearchAsync<Properties>(s => s.Index("properties, mgmt")
                .MinScore(0.5)
                .Query(q => q
                    .Bool(b => b
                            .Must(mu => mu
                                    .Match(m => m
                                        .Field(f => f.Property.Name)
                                        .Query(query).Boost(20)
                                    ), mu => mu
                                    .Match(m => m
                                        .Field(f => f.Property.FormerName)
                                        .Query(query).Boost(25)
                                    )
                            )
                            .Filter(fi => fi.Match(m => m
                                .Field(f => f.Property.Market)
                                .Query(marketList)))
                        //if marketList is List<string> then use ExtractString()
                    )
                )
            );
        }

        public async Task<List<Properties>> AutoCompleteTask(string query)
        {
            var searchResponse = await _client.SearchAsync<Properties>(s => s
                .Index("properties")
                .Suggest(su => su
                    .Completion("suggestions", c => c
                        .Field(f => f.Suggest)
                        .Prefix(query)
                        .Fuzzy(f => f
                            .Fuzziness(Fuzziness.Auto)
                        )
                        .Analyzer("simple")
                        .Size(5))
                ));

            return searchResponse.Suggest["suggestions"].SelectMany(suggest => suggest.Options).Select(suggestOption => suggestOption.Source).ToList();
        }

        private static string ExtractString(IReadOnlyCollection<string> marketList)
        {
            if (marketList == null || marketList.Count < 0)
                return null;

            var sb = new StringBuilder();

            foreach (var market in marketList)
            {
                sb.Append(market);
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }
    }
}
