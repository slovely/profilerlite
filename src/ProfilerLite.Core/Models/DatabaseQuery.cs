using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ProfilerLite.Core.Models
{
    public class DatabaseQuery
    {
        public int Id { get; set; }
        public string CommandText { get; set; }
        public string CommandTextParameterized => GetCommandTextParameterized();
        public int Rows { get; set; }
        public int Time { get; set; }
        public string TimeFormatted => Time.ToHumanReadableTime();

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
}