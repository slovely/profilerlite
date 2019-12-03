using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProfilerLite.Core.Models;

namespace ProfilerLite.Core.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly DataProvider _dataProvider;

        public DataController(DataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        [HttpGet]
        [Route("api/[controller]/sessions")]
        public async Task<List<DatabaseSessionSummary>> Sessions()
        {
            return await _dataProvider.GetSessionsAsync();
        }

        [HttpGet]
        [Route("api/[controller]/sessiondetail/{id}")]
        public async Task<DatabaseSessionDetail> SessionDetail(int id)
        {
            return await _dataProvider.GetSessionDetail(id);
        }
    }
}