using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Links
    {
        [Key]
        public int Id { get; set; }
        public Self? Self { get; set; }
        [JsonPropertyName("previousepisode")]
        public PreviousEpisode? PreviousEpisode { get; set; }
        public int? ShowId { get; set; }
        public Show? Show { get; set; }
    }
}
