using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamModel>> GetTeams();
        Task<List<StandingsModel>> GetStandings();
        Task<StandingsModel> GetStandingById(int id);
        Task<TeamStatModel> UpdateTeamStats(TeamStatModel model);
        Task<ResultModel> GetResultById(int id);
        Task<ResultModel> UpdateResult(ResultModel model);
        Task<ResultModel> ResetResult(ResultModel model);
        
    }
}
