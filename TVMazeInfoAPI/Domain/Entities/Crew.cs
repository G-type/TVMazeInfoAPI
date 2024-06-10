using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Crew
    {
        [Key]
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public Person? Person { get; set; }
        public string? Type { get; set; }
        public int? ShowId { get; set; } 
        public Show? Show { get; set; } 
    }
}