using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public double? Average { get; set; }
        public int? ShowId { get; set; }
        public Show? Show { get; set; }
    }
}
