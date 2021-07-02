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
    public class AutoCompleteController : Controller
    {
        private readonly ILogger<AutoCompleteController> _logger;
        private readonly IElastiApiMethods _client;

        public AutoCompleteController(ILogger<AutoCompleteController> logger, IElastiApiMethods client) =>
            (_logger, _client) = (logger, client);

        [HttpPost]
        public async Task<List<Properties>> Index(string query)
        {
            var responseList = await _client.AutoCompleteTask(query);

            if (responseList.Count >= 0)
                return responseList;

            _logger.LogError("Autocomplete couldn't be made");
            return await Task.FromResult<List<Properties>>(null).ConfigureAwait(false);
        }
    }
}
