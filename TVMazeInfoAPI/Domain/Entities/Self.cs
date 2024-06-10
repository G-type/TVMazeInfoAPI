using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Self
    {
        [Key]
        public int Id { get; set; }
        public string? Href { get; set; }
    }
}
