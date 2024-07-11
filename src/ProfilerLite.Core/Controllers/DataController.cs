using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<List<DatabaseSessionSummary>> Sessions([FromQuery] string urlFilter = "")
        {
            return await _dataProvider.GetSessionsAsync(urlFilter);
        }

        [HttpGet]
        [Route("api/[controller]/sessiondetail/{id}")]
        public async Task<DatabaseSessionDetail> SessionDetail(int id)
        {
            return await _dataProvider.GetSessionDetail(id);
        }
        
        [HttpPost]
        [Route("api/[controller]/cleardatabase")]
        public async Task<bool> ClearDatabase()
        {
            await _dataProvider.TruncateTables();
            return true;
        }

        [HttpGet]
        [Route("api/[controller]/downloadallqueries/{id}")]
        public async Task<ActionResult> DownloadAllQueries(int id)
        {
            var details = await _dataProvider.GetSessionDetail(id);
            // return a text file with all the queries in it
            var content = new StringBuilder();

            content.AppendLine("BEGIN TRAN");
            content.AppendLine();
            foreach (var query in details.DatabaseQueries.OrderBy(x => x.Id))
            {
                content.AppendLine("-- QUERYID: " + query.Id);
                content.AppendLine(query.CommandTextParameterized);
                content.AppendLine();
                content.AppendLine();
            }

            content.AppendLine();
            content.AppendLine("ROLLBACK TRAN");

            var bytes = Encoding.UTF8.GetBytes(content.ToString());
            return File(bytes, "text/plain", "queries-" + id + ".txt");
        }
    }
}