using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionsWithStandingsModel
    {
        [Key]
        public int Id { get; set; }
        public PredictionStandingsModel? Standing { get; set; }
        public List<PredictionModel>? Predictions { get; set; }
    }
}
