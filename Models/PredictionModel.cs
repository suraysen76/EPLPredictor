using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Fixture Id ")]
        public int FixtureId { get; set; }
        [DisplayName("User Name")]
        public string? UserName { get; set; }
        //public string? HomeTeam { get; set; }
        //public string? AwayTeam { get; set; }
        [DisplayName("Home Team Score")]
        public int? HomeTeamScore { get; set; }
        [DisplayName("Away Team Score")]
        public int? AwayTeamScore { get; set; }

        [DisplayName("Uodated Date")]
        public DateTime UpdatedDate { get; set; }
       
    }
}
