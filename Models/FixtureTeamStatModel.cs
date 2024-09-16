using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class FixtureTeamStatModel
    {
        
        public List<FixtureModel>? Fixtures { get; set; }
        //public string ActualResult { get; set; }
        public TeamStatModel? TeamStat { get; set; }

        [NotMapped]
        public string? Team { get; set; }
    }
}
