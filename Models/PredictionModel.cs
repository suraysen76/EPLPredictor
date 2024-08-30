using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionModel
    {
        [Key]
        public int Id { get; set; }
        public int MatchWeek { get; set; }
        public string UserName { get; set; }
        public string Prediction { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ScoreModel Score { get; set; }
    }
}
