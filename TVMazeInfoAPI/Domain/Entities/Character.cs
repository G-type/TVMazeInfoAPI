

using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Image? Image { get; set; }
    }
}
