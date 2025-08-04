using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdminPanel.Models.Organization.User
{
    public class AddUser
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("mail")]
        public string Mail { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("roles")]
        public List<int> Roles { get; set; } = new List<int>();
    }
}
