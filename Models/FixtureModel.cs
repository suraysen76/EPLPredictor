using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class FixtureModel
    {
        [Key]
        public int Id { get; set; }
        
        public int MatchWeek { get; set; }
        public DateTime Date { get; set; }

        public string? Location  { get; set; }
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }

        public string? Result { get; set; }

        public bool? IsLocked { get; set; }

    }
}
