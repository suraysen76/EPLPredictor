using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class PointModel
    {
        [Key]
        public int Id { get; set; }
        public int PredictionId { get; set; }
        public int Point { get; set; }
    }
}
