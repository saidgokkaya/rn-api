using System.Text.Json.Serialization;

namespace WebApi.Models.Numarataj
{
    public class DeleteNumberings
    {
        [JsonPropertyName("numberingIds")]
        public List<int> NumberingIds { get; set; } = new List<int>();
    }
}
