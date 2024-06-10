using System.ComponentModel.DataAnnotations;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class WebChannel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Country? Country { get; set; }
        public string? OfficialSite { get; set; }
    }
}
