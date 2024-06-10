using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }
        public int? Season { get; set; }
        public int? Number { get; set; }
        public string? Name { get; set; }
        public string? Airdate { get; set; }
        public string? Runtime { get; set; }
        public string? Summary { get; set; }
        public Image? Image { get; set; }

        public int ShowId { get; set; }
        public Show? Show { get; set; }
    }
}