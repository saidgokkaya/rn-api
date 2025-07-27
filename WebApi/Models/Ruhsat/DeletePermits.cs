using System.Text.Json.Serialization;

namespace WebApi.Models.Ruhsat
{
    public class DeletePermits
    {
        [JsonPropertyName("permitIds")]
        public List<int> PermitIds { get; set; } = new List<int>();
    }
}
