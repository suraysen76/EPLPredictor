using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionWinnersModel
    {
        [Key]
        public int Id { get; set; }

        public int FixtureId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public int? Point { get; set; }
    }
}
