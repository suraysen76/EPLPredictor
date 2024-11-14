using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class PredictionModel
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public DateTime FixtureDate { get; set; }
        [DisplayName("Fixture Id ")]
        public int FixtureId { get; set; }

        public int UserId { get; set; }
        [DisplayName("User Name")]
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public string? HomeTeam { get; set; }
        [NotMapped] 
        public string? AwayTeam { get; set; }
        [DisplayName("Home Team Score")]
        public int? HomeTeamScore { get; set; }
        [DisplayName("Away Team Score")]
        public int? AwayTeamScore { get; set; }
        [NotMapped]
        public int? Point { get; set; }
        [DisplayName("Uodated Date")]
        public DateTime UpdatedDate { get; set; }
       
    }
}
