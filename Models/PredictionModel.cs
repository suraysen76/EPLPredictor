using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionModel
    {
        [Key]
        public int Id { get; set; }
        public int FixtureId { get; set; }
        public string? UserName { get; set; }
        //public string? HomeTeam { get; set; }
        //public string? AwayTeam { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public DateTime UpdatedDate { get; set; }
       
    }
}
