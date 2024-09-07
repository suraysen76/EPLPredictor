using System.ComponentModel.DataAnnotations;
namespace SS1892.EPLPredictor.Models
{
    public class PredictionStandingsModel
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public int? TotalPoints { get; set; }



    }
}
