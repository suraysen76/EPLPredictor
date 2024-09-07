using Microsoft.CodeAnalysis;

namespace SS1892.EPLPredictor.Models
{
    public class StandingsModel
    {
        public TeamStatModel? TeamsStats { get; set; }
        public TeamModel? Team { get; set; }
    }
}

