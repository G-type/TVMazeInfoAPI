using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class PreviousEpisode
    {
        [Key]
        public int Id { get; set; }
        public string? Href { get; set; }
        public string? Name { get; set; }
    }
}
