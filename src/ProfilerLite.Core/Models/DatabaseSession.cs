using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
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

    public class DatabaseSessionDetail : DatabaseSessionSummary
    {
        public DatabaseSessionDetail()
        {
            DatabaseQueries = new List<DatabaseQuery>();
        }

        public List<DatabaseQuery> DatabaseQueries { get; set; }
    }

    public class DatabaseQuery
    {
        public int Id { get; set; }
        public string CommandText { get; set; }
        public string CommandTextParameterized => GetCommandTextParameterized();
        public int Rows { get; set; }
        public int Time { get; set; }
        public string TimeFormatted => GetTimeFormatted();

        private string GetTimeFormatted()
        {
            if (Time < 1000) return $"{Time}ms";
            if (Time < 60000) return $"{Math.Round(Time / 1000d, 2)}s";
            return $"{Math.Round(Time / 60000d, 2)}mins";
        }

        public List<QueryParameter> ParametersDeserialized => JsonSerializer.Deserialize<Dictionary<string, string>>(Parameters)
            .Select(kvp => new QueryParameter(kvp.Key, kvp.Value))
            .ToList();
        public string Parameters { get; set; }
        
        private string GetCommandTextParameterized()
        {
            var result = CommandText;
            foreach (var param in ParametersDeserialized)
            {
                var paramValue = param.Value;
                paramValue = FormatParam(paramValue);
                var paramName = param.Name.StartsWith("@") ? param.Name : "@" + param.Name;
                result = result.Replace(paramName, paramValue + $" /* {paramName} */ ");
            }
            return result;
        }

        private string FormatParam(string paramValue)
        {
            if (int.TryParse(paramValue, out _)) return paramValue;
            return "'" + paramValue + "'";
        }
    }

    public class QueryParameter
    {
        public QueryParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
    
}