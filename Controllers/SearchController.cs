using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElastiRestApi.API;
using Microsoft.Extensions.Logging;
using Properties = ElastiRestApi.Business.JsonObjects.Properties;

namespace ElastiRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly IElastiApiMethods _client;

        public SearchController(ILogger<SearchController> logger, IElastiApiMethods client) =>
            (_logger, _client) = (logger, client);

        [HttpPost]
        public async Task<IReadOnlyCollection<Properties>> Index(string query, string marketList = null)
        {
            var response = await _client.SearchTask(query, marketList);

            if (response.IsValid)
                return response.Documents;

            _logger.LogError("Search couldn't be completed");
            return await Task.FromResult<IReadOnlyCollection<Properties>>(null).ConfigureAwait(false);
        }
    }
}
