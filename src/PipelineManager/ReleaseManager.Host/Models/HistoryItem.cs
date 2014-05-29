using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReleaseManager.Host.Models
{
    public class HistoryItem
    {
        public string Date { get; set; }
        public string Message { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public HistoryItemType Type { get; set; }
    }
}