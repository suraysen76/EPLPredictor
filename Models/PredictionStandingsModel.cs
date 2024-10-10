using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionStandingsModel
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        [DisplayName("User Name")]
        public string? Username { get; set; }
        public string? Name { get; set; }

        [DisplayName("Total Points")]
        public int? TotalPoints { get; set; }

        [NotMapped]
        public int? Rank { get; set; }




    }
}
