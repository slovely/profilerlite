using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

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
                result = Regex.Replace(result, paramName + "\\b", paramValue);
            }
            return result;
        }

        private string FormatParam(string paramValue)
        {
            if (int.TryParse(paramValue, out _)) return paramValue;
            if (bool.TryParse(paramValue, out var b)) return b ? "1" : "0";
            if (DateTime.TryParseExact(paramValue, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out var date)) return "'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            return "'" + paramValue + "'";
        }
    }
}