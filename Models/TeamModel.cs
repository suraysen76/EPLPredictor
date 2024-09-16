using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class TeamModel
    {
        //[Key]
        [Key]
        public int Id { get; set; }

        public string? Team { get; set; }

    }
}
