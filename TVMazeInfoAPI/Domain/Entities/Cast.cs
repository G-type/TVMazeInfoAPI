using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public Person? Person { get; set; }
        public int? CharacterId { get; set; }
        public Character? Character { get; set; }
        public int? ShowId { get; set; }
        public Show? Show { get; set; }
    }
}