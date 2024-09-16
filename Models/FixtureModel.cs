using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class FixtureModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Match Week")]
        public int MatchWeek { get; set; }
        public DateTime Date { get; set; }

        public string? Location  { get; set; }
        [DisplayName("Home Team")]
        public int? HomeTeamId { get; set; }
        [DisplayName("Away Team")]
        public int? AwayTeamId { get; set; }

        public string? Result { get; set; }

        public bool? IsLocked { get; set; }

        [NotMapped]
        public string? HomeTeam { get; set; }

        [NotMapped]
        public string? AwayTeam { get; set; }
    }
}
