using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SS1892.EPLPredictor.Handler;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Migrations;
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

        public async Task<List<TeamModel>> GetTeams(string type)
        {
            var teams = await _context.Teams
                .Where(o=>o.Type == type)
                .OrderBy(o=>o.Team)
                .ToListAsync();
            return  teams;
        }

        public async Task<List<StandingsModel>> GetStandings(string type)
        {
            var viewModel =
            from ts in _context.TeamStats.DefaultIfEmpty()
            join t in _context.Teams on ts.TeamId equals t.Id
            where t.Type==type
            orderby ts.Points
            //where ts.TeamId ==3 || ts.TeamId ==15 || ts.TeamId == 1

            select new StandingsModel { TeamsStats = ts, Team = t };


            //var tmodel=viewModel;

            //foreach (var team in tmodel)
            //{
            //    var winpoints = team.TeamsStats.Win * 3;
            //    var drawpoints = team.TeamsStats.Draw * 1;
            //    team.TeamsStats.Points = winpoints + drawpoints;
            //    team.TeamsStats.GD = team.TeamsStats.GF - team.TeamsStats.GA;
            //}
            var tmodel = viewModel
                .OrderByDescending(t => t.TeamsStats.Points)            
                //.ThenByDescending(t => t.TeamsStats.Win)
                //.ThenByDescending(t => t.TeamsStats.Draw)
                .ThenByDescending(t => t.TeamsStats.GD)
                .ThenByDescending(t => t.TeamsStats.GF)
                //.ThenBy(t=>t.TeamsStats.GA)
                .ThenBy(t=>t.Team.Team)
                ;
            
            
            return await tmodel.ToListAsync();
        }
        
        public async Task<FixtureTeamStatModel> GetStandingById(int id)
        {
            var ftmodel = new FixtureTeamStatModel();
            var tsModel = await _context.TeamStats.Where(t => t.TeamId == id).FirstOrDefaultAsync();
            
            //var winpoints = tsModel.Win * 3;
            //var drawpoints = tsModel.Draw * 1;
            //tsModel.Points = winpoints + drawpoints;

            var tmodel= await _context.Teams.Where(t => t.Id == id).FirstOrDefaultAsync();
            var fmodel =
            from f in _context.Fixtures
            .Where(f=>(f.HomeTeamId==id || f.AwayTeamId==id) && f.IsLocked==true)
            .OrderBy(f => f.Date)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Location = f.Location,
                HomeTeamId = f.HomeTeamId,
                AwayTeamId = f.AwayTeamId,
                HomeTeam = t1.Team,
                AwayTeam = t2.Team,
                Result = f.Result,
                IsLocked = f.IsLocked
            };

            ftmodel.TeamStat = tsModel;
            ftmodel.Fixtures=fmodel.ToList();
            ftmodel.Team = tmodel.Team;
            return ftmodel;
        }

        public async Task<TeamStatModel> UpdateTeamStats(TeamStatModel model)
        {

            var viewModel = _context.TeamStats.Where(t => t.TeamId == model.TeamId).FirstOrDefault();

            //viewModel.Win=model.Win;
            //viewModel.Draw = model.Draw;
            //viewModel.Lost = model.Lost;
            //viewModel.GF = model.GF;
            //viewModel.GA = model.GA;

            //var winpoints = viewModel.Win * 3;
            //var drawpoints = viewModel.Draw * 1;
            //viewModel.Points = winpoints + drawpoints;

            await _context.SaveChangesAsync();
            return viewModel;
            
        }

        public async Task<ResultModel> GetResultById(int id)
        {
            var viewModel =
            from r in _context.Results.DefaultIfEmpty()
            join f in _context.Fixtures on r.FixtureId equals f.Id
            join t1 in _context.Teams on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams on f.AwayTeamId equals t2.Id
            where r.FixtureId == id
            select new ResultModel{ 
                AwayTeam=t2.Team,
                HomeTeam=t1.Team,
                FixtureId=f.Id,
                Type =f.Type,
                AwayTeamScore=r.AwayTeamScore,
                HomeTeamScore=r.HomeTeamScore,
                Fixture= t1.Team + " vs "+ t2.Team,

            };

            var retModel = _context.Results.Where(r=>r.FixtureId==id).FirstOrDefault();
            
           
            return viewModel.FirstOrDefault();
        }

        public async Task<ResultModel> UpdateResult(ResultModel model)
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

            //Update Results
            resultModel.AwayTeamScore = model.AwayTeamScore;
            resultModel.HomeTeamScore = model.HomeTeamScore;

            //Update Fixture Result
            var fixtureModel2 = _context.Fixtures.Where(f => f.Id == model.FixtureId).FirstOrDefault();
            fixtureModel2.Result = resultModel.HomeTeamScore.ToString() + " - " + resultModel.AwayTeamScore.ToString();
            fixtureModel2.IsLocked = true;
            int? homeTeamId = fixtureModel2.HomeTeamId.HasValue ? fixtureModel2.HomeTeamId : null; 
            int? awayTeamId =  fixtureModel2.AwayTeamId.HasValue ? fixtureModel2.AwayTeamId : null;

            //Update  TeamStats
            var homeModel = _context.TeamStats.Where(t => t.TeamId == homeTeamId).FirstOrDefault();
            var awayModel = _context.TeamStats.Where(t => t.TeamId == awayTeamId).FirstOrDefault();
            homeModel.GF = homeModel.GF + resultModel.HomeTeamScore;
            homeModel.GA = homeModel.GA + resultModel.AwayTeamScore;
            homeModel.GD = homeModel.GF - homeModel.GA;
            awayModel.GF = awayModel.GF + resultModel.AwayTeamScore;
            awayModel.GA = awayModel.GA + resultModel.HomeTeamScore;
            awayModel.GD = awayModel.GF - awayModel.GA;
            

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
            homeModel.Points = (homeModel.Win) * 3 + (homeModel.Draw * 1);
            awayModel.Points = (awayModel.Win) * 3 + (awayModel.Draw * 1);

            //Update PredictionWinners
            var winnerList = await _context.PredictionWinners.Where(w => w.FixtureId == model.FixtureId).ToListAsync();
            var pmodel = await _context.Predictions.Where(p => p.FixtureId == model.FixtureId).ToListAsync();
            var fmodel = await _context.Fixtures.Where(f => f.Id == model.FixtureId).FirstOrDefaultAsync();
            var actualResult = fmodel.Result.Split("-");
            var homeResult = actualResult[0].Trim();
            var awayResult = actualResult[1].Trim();
            var pt3winners = pmodel.Where(p => p.HomeTeamScore.ToString() == homeResult && p.AwayTeamScore.ToString() == awayResult).ToList();
            foreach (var item in pt3winners)
            {
                var user = await _context.Users
                    .Where(u => u.Id == item.UserId).FirstOrDefaultAsync();

                var winner = new PredictionWinnersModel() { 
                    FixtureId = model.FixtureId, 
                    Name = user.Name, 
                    UserName = user.UserName, 
                    UserId  =   user.Id,
                    Point = 3 };
                _context.PredictionWinners.Add(winner);
            }

            if (isHomeWin)
            {
                var pt1homewinners = pmodel
                    .Where(p => p.HomeTeamScore > p.AwayTeamScore)
                    .Except(pt3winners)
                    .ToList();
                foreach (var item in pt1homewinners)
                {
                    var user = await _context.Users
                        .Where(u => u.Id == item.UserId).FirstOrDefaultAsync();
                    var winner = new PredictionWinnersModel() { 
                        FixtureId = model.FixtureId, 
                        UserId = user.Id,
                        Name = user.Name,
                        Point = 1 };
                    _context.PredictionWinners.Add(winner);
                }
            }

            else if (isAwayWin)
            {
                var pt1awaywinners = pmodel
                    .Where(p => p.HomeTeamScore < p.AwayTeamScore)
                    .Except(pt3winners)
                    .ToList();
                foreach (var item in pt1awaywinners)
                {
                    var user = await _context.Users.Where(u => u.Id == item.UserId).FirstOrDefaultAsync();
                    var winner = new PredictionWinnersModel() { 
                        FixtureId = model.FixtureId,
                        UserId = user.Id,
                        Name = user.Name, 
                        UserName = user.UserName, 
                        Point = 1 };
                    _context.PredictionWinners.Add(winner);
                }
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
            int? homeTeamId = fixtureModel2.HomeTeamId.HasValue ? fixtureModel2.HomeTeamId : null;
            int? awayTeamId = fixtureModel2.AwayTeamId.HasValue ? fixtureModel2.AwayTeamId : null;

            //Update  TeamStats
            var homeModel = _context.TeamStats.Where(t => t.TeamId == homeTeamId).FirstOrDefault();
            var awayModel = _context.TeamStats.Where(t => t.TeamId == awayTeamId).FirstOrDefault();
            homeModel.GF = homeModel.GF - resultModel.HomeTeamScore;
            homeModel.GA = homeModel.GA - resultModel.AwayTeamScore;
            homeModel.GD = homeModel.GF - homeModel.GA;
            awayModel.GF = awayModel.GF - resultModel.AwayTeamScore;
            awayModel.GA = awayModel.GA - resultModel.HomeTeamScore;
            awayModel.GD = awayModel.GF - awayModel.GA;


            //Update PredictionWinners
            var winners = _context.PredictionWinners.Where(w => w.FixtureId == model.FixtureId);
            foreach(var winner in winners)
            {
                _context.PredictionWinners.Remove(winner);
            }
            

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
        public async Task<ErrorViewModel> AddTeam(TeamModel model)
        {
            var response = new ErrorViewModel();
            model.Team = model.Team.Trim();
            try
            {
                var exist = _context.Teams.Where(x=>x.Type==model.Type && x.Team==model.Team).Any();
                if (exist) {
                    response.Status = false;
                    response.Message = "Team "+ model.Team + " already exist in "+model.Type ;
                }
                else
                {
                    _context.Teams.Add(model);
                    _context.SaveChanges();
                    response.Status = true;
                    response.Message = "Added successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = true;
                response.Message = "Error :"+ ex.ToString();
            }
            return response;
        }

        public async Task<TeamModel> GetTeamById(int teamId)
        {
            var team = await _context.Teams
                .Where(o => o.Id == teamId)                
                .FirstOrDefaultAsync();
            return team;
        }

        
    }
}

