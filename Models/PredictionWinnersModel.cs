using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionWinnersModel
    {
        [Key]
        public int Id { get; set; }

        public int FixtureId { get; set; }
        public int UserId { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public int? Point { get; set; }
      
    }
}
