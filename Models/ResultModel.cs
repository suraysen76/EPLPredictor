using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class ResultModel
    {
        [Key]
        public int Id { get; set; }

        public int FixtureId { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
    }
}
