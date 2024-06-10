using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string? Medium { get; set; }
        public string? Original { get; set; }
        public int? ShowId { get; set; }
        public Show? Show { get; set; }
    }
}
