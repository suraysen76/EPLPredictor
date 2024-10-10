using Microsoft.AspNetCore.Mvc.Rendering;
using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamModel>> GetTeams(string type);

        Task<TeamModel> GetTeamById(int teamId);
        Task<List<StandingsModel>> GetStandings(string type);
        Task<FixtureTeamStatModel> GetStandingById(int id);
        Task<TeamStatModel> UpdateTeamStats(TeamStatModel model);

        Task<ErrorViewModel> AddTeam(TeamModel model);
        Task<ResultModel> GetResultById(int id);
        Task<ResultModel> UpdateResult(ResultModel model);
        Task<ResultModel> ResetResult(ResultModel model);

        


    }
}
