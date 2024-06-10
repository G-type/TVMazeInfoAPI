using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Externals
    {
        [Key]
        public int Id { get; set; }
        public int? Tvrage { get; set; }
        public int? Thetvdb { get; set; }
        public string? Imdb { get; set; }
        public int? ShowId { get; set; }
        public Show? Show { get; set; }
    }
}
