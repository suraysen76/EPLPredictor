using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class TeamModel
    {
        [Key]
        public int Id { get; set; }

        public string Team { get; set; }
    }
}
