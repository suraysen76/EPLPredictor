using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class ScoreModel
    {
        [Key]
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Score { get; set; }
    }
}
