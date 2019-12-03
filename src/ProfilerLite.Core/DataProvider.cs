using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using ProfilerLite.Core.Models;

namespace ProfilerLite.Core
{
    public class DataProvider
    {
        private readonly IOptions<Configuration> _config;

        public DataProvider(IOptions<Configuration> config)
        {
            _config = config;
        }

        public async Task<List<DatabaseSessionSummary>> GetSessionsAsync()
        {
            var sql = @"
select top 20 s.*, count(sq.id) QueryCount
from __Session s
    join __SessionQuery sq on sq.SessionId = s.SessionId
group by s.Id, s.SessionId, s.Url, s.Method, s.CreatedDate
order by CreatedDate desc
";

            using var conn = new SqlConnection(_config.Value.ConnectionStrings.SqlLogDb);
            await conn.OpenAsync();
            return (await conn.QueryAsync<DatabaseSessionSummary>(sql)).ToList();
        }

        public async Task<DatabaseSessionDetail> GetSessionDetail(int sessionId)
        {
            var sql = @"
select s.*
from __Session s
where s.id = @id; 

select sq.*
from __SessionQuery sq
where sq.sessionid = (select sessionId from __session where __session.id = @id) 
order by sq.id;
";
            using var conn = new SqlConnection(_config.Value.ConnectionStrings.SqlLogDb);
            await conn.OpenAsync();
            using var multi = await conn.QueryMultipleAsync(sql, new {id = sessionId});
            var result = (await multi.ReadAsync<DatabaseSessionDetail>()).FirstOrDefault();
            result.DatabaseQueries = (await multi.ReadAsync<DatabaseQuery>()).ToList();
            return result;
        }
    }
}