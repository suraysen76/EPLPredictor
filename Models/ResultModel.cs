using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class ResultModel
    {
        [Key]
        public int Id { get; set; }

        public int FixtureId { get; set; }
        public string Type { get; set; }
        [DisplayName("Home Team Score")]
        public int? HomeTeamScore { get; set; }
        [DisplayName("Away Team Score")]
        public int? AwayTeamScore { get; set; }

        [NotMapped]
        public string Fixture { get; set; }
        [NotMapped]
        public string HomeTeam { get; set; }
        [NotMapped]
        public string AwayTeam { get; set; }
    }
}
