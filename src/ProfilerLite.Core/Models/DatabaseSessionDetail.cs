using System.Collections.Generic;
using System.Linq;

namespace ProfilerLite.Core.Models
{
    public class DatabaseSessionDetail : DatabaseSessionSummary
    {
        public DatabaseSessionDetail()
        {
            DatabaseQueries = new List<DatabaseQuery>();
        }

        public List<DatabaseQuery> DatabaseQueries { get; set; }
        public int TotalRowCount => DatabaseQueries.Sum(x => x.Rows);
        public string TotalDatabaseTimeFormatted => DatabaseQueries.Sum(x => x.Time).ToHumanReadableTime();
    }
}