using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using Properties = ElastiRestApi.Business.JsonObjects.Properties;

namespace ElastiRestApi.API
{
    public interface IElastiApiMethods
    {
        Task<ISearchResponse<Properties>> SearchTask(string query, string marketList);
        Task<List<Properties>> AutoCompleteTask(string query);
    }
}
