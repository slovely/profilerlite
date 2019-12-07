namespace ProfilerLite.Core.Models
{
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