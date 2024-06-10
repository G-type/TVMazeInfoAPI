using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Country
    {
        [Key]
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Timezone { get; set; }
    }
}
