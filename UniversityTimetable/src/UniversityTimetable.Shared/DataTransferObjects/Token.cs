using System.Text.Json.Serialization;

namespace UniversityTimetable.Shared.DataTransferObjects
{
    public class Token
    {
        public string Email { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
