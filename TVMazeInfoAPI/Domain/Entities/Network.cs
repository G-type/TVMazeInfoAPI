using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVMazeInfoAPI.Domain.Entities
{
    public class Network
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        [ForeignKey("Country")]
        public string? CountryCode { get; set; }
        public Country? Country { get; set; }
    }
}