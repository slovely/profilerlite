using System;
using System.Text.RegularExpressions;

namespace ProfilerLite.Core.Models
{
    public class DatabaseSessionSummary
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string ShortenedUrl => Regex.Replace(Url, "^(http[s]?:\\/\\/)[\\w.-]*\\/(.*)$", "$1...$2"); 
        public string Method { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QueryCount { get; set; }
    }
}