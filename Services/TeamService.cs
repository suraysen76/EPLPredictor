using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SS1892.EPLPredictor.Handler;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;
using System.Runtime.CompilerServices;

namespace SS1892.EPLPredictor.Services
{
    public class TeamService:ITeamService
    {
        private readonly DBCtx _context;

        public TeamService(DBCtx context)
        {
            _context = context;
        }

        public async Task<List<TeamModel>> GetTeams()
        {
            var teams = await _context.Teams.ToListAsync();
            return  teams;
        }

        public async Task<List<StandingsModel>> GetStandings()
        {
            var viewModel =
            from ts in _context.TeamStats.DefaultIfEmpty()
            join t in _context.Teams on ts.TeamId equals t.Id
            //where ts.TeamId ==3 || ts.TeamId ==15 || ts.TeamId == 1

            select new StandingsModel { TeamsStats = ts, Team = t };
           

            var retModel = viewModel;
            foreach (var team in retModel)
            {
                var winpoints = team.TeamsStats.Win * 3;
                var drawpoints = team.TeamsStats.Draw * 1;
                team.TeamsStats.Points = winpoints + drawpoints;
            }
            var model1 = retModel
                .OrderByDescending(t => t.TeamsStats.Points)            
                .ThenByDescending(t => t.TeamsStats.Win)
                .ThenByDescending(t => t.TeamsStats.Draw)
                .ThenByDescending(t => t.TeamsStats.GF- t.TeamsStats.GA)
                .ThenByDescending(t => t.TeamsStats.GF)
                .ThenBy(t=>t.TeamsStats.GA)
                .ThenBy(t=>t.Team.Team);
            
            
            return model1.ToList();
        }
        
        public async Task<StandingsModel> GetStandingById(int id)
        {
            var viewModel =
            from ts in _context.TeamStats.DefaultIfEmpty()
            join t in _context.Teams on ts.TeamId equals t.Id
            where t.Id == id
            select new StandingsModel { TeamsStats = ts, Team = t };

            var retModel = viewModel;
            return retModel.FirstOrDefault();
        }

        public async Task<TeamStatModel> UpdateTeamStats(TeamStatModel model)
        {

            var viewModel = _context.TeamStats.Where(t => t.TeamId == model.TeamId).FirstOrDefault();

            viewModel.Win=model.Win;
            viewModel.Draw = model.Draw;
            viewModel.Lost = model.Lost;
            viewModel.GF = model.GF;
            viewModel.GA = model.GA;
            await _context.SaveChangesAsync();
            return viewModel;
            
        }

        public async Task<ResultModel> GetResultById(int id)
        {
            var viewModel =
            from r in _context.Results.DefaultIfEmpty()
            join f in _context.Fixtures on r.FixtureId equals f.Id
            where f.Id == id
            select new { TeamsStats = r, Team = f };

            var retModel = _context.Results.Where(r=>r.FixtureId==id).FirstOrDefault();
           
            return retModel;
        }

        public async Task<ResultModel> UpdateResult(ResultModel model)
        {
            bool isHomeWin = false;
            bool isAwayWin = false;
            bool isDraw = false;
            if (model.HomeTeamScore> model.AwayTeamScore)
            {
                isHomeWin = true;
            }
            else if (model.AwayTeamScore > model.HomeTeamScore)
            {
                isAwayWin = true;
            }
            else if (model.AwayTeamScore == model.HomeTeamScore)
            {
                isDraw = true;
            }


            var resultModel = _context.Results.Where(t => t.FixtureId == model.FixtureId).FirstOrDefault();

            //Update Results
            resultModel.AwayTeamScore = model.AwayTeamScore;
            resultModel.HomeTeamScore = model.HomeTeamScore;

            //Update Fixture Result
            var fixtureModel2 = _context.Fixtures.Where(f => f.Id == model.FixtureId).FirstOrDefault();
            fixtureModel2.Result = resultModel.HomeTeamScore.ToString() + " - "+ resultModel.AwayTeamScore.ToString();
            fixtureModel2.IsLocked = true;
            int homeTeamId = _context.Teams.Where(t => t.Team.Equals(fixtureModel2.HomeTeam)).FirstOrDefault().Id;
            int awayTeamId = _context.Teams.Where(t => t.Team.Equals(fixtureModel2.AwayTeam)).FirstOrDefault().Id;

            //Update  TeamStats
            var homeModel = _context.TeamStats.Where(t => t.TeamId == homeTeamId).FirstOrDefault();
            var awayModel = _context.TeamStats.Where(t => t.TeamId == awayTeamId).FirstOrDefault();
            homeModel.GF = homeModel.GF + resultModel.HomeTeamScore;
            homeModel.GA = homeModel.GA + resultModel.AwayTeamScore;
            awayModel.GF = awayModel.GF + resultModel.AwayTeamScore;
            awayModel.GA = awayModel.GA + resultModel.HomeTeamScore;


            if (isHomeWin)
            {
                homeModel.Win++;
                awayModel.Lost++;
            }
            else if (isAwayWin)
            {
                awayModel.Win++;
                homeModel.Lost++;
            }
            else if (isDraw)
            {
                homeModel.Draw++;
                awayModel.Draw++;
            }
            await _context.SaveChangesAsync();
            return resultModel;

        }

        public async Task<ResultModel> ResetResult(ResultModel model)
        {
            bool isHomeWin = false;
            bool isAwayWin = false;
            bool isDraw = false;
            if (model.HomeTeamScore > model.AwayTeamScore)
            {
                isHomeWin = true;
            }
            else if (model.AwayTeamScore > model.HomeTeamScore)
            {
                isAwayWin = true;
            }
            else if (model.AwayTeamScore == model.HomeTeamScore)
            {
                isDraw = true;
            }


            var resultModel = _context.Results.Where(t => t.FixtureId == model.FixtureId).FirstOrDefault();

            

            //Update Fixture Result
            var fixtureModel2 = _context.Fixtures.Where(f => f.Id == model.FixtureId).FirstOrDefault();
            fixtureModel2.Result = null;
            fixtureModel2.IsLocked = false;
            int homeTeamId = _context.Teams.Where(t => t.Team.Equals(fixtureModel2.HomeTeam)).FirstOrDefault().Id;
            int awayTeamId = _context.Teams.Where(t => t.Team.Equals(fixtureModel2.AwayTeam)).FirstOrDefault().Id;

            //Update  TeamStats
            var homeModel = _context.TeamStats.Where(t => t.TeamId == homeTeamId).FirstOrDefault();
            var awayModel = _context.TeamStats.Where(t => t.TeamId == awayTeamId).FirstOrDefault();
            homeModel.GF = homeModel.GF - resultModel.HomeTeamScore;
            homeModel.GA = homeModel.GA - resultModel.AwayTeamScore;
            awayModel.GF = awayModel.GF - resultModel.AwayTeamScore;
            awayModel.GA = awayModel.GA - resultModel.HomeTeamScore;

            //Update Results
            resultModel.AwayTeamScore = null;
            resultModel.HomeTeamScore = null;
            if (isHomeWin)
            {
                homeModel.Win--;
                awayModel.Lost--;
            }
            else if (isAwayWin)
            {
                awayModel.Win--;
                homeModel.Lost--;
            }
            else if (isDraw)
            {
                homeModel.Draw--;
                awayModel.Draw--;
            }
            await _context.SaveChangesAsync();
            return resultModel;

        }
    }





}

