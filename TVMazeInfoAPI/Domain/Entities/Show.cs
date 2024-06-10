using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Show
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("language")]
        public string? Language { get; set; }
        [JsonPropertyName("genres")]
        public List<string>? Genres { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; }
        [JsonPropertyName("averageRuntime")]
        public int? AverageRuntime { get; set; }
        [JsonPropertyName("premiered")]
        public string? Premiered { get; set; }
        [JsonPropertyName("ended")]
        public string? Ended { get; set; }
        [JsonPropertyName("officialSite")]
        public string? OfficialSite { get; set; }
        public Rating? Rating { get; set; }
        [JsonPropertyName("weight")]
        public int? Weight { get; set; }
        public Network? Network { get; set; }
        [JsonPropertyName("webChannel")]
        public WebChannel? WebChannel { get; set; }
        public string? DvdCountry { get; set; }
        public Externals? Externals { get; set; }
        public Image? Image { get; set; }
        [JsonPropertyName("summary")]
        public string? Summary { get; set; }
        [JsonPropertyName("updated")]
        public int? Updated { get; set; }
        [JsonPropertyName("networkId")]
        public int? NetworkId { get; set; }
        [JsonPropertyName("_links")]
        public Links? _links { get; set; }
        public List<Episode>? Episodes { get; set; }
        public List<Cast>? Cast { get; set; }
        public List<Crew>? Crew { get; set; }
    }
}