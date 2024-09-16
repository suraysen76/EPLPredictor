using Microsoft.EntityFrameworkCore;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Services
{
    public class PredictionService : IPredictionService
    {

        private readonly DBCtx _context;

        public PredictionService(DBCtx context)
        {
            _context = context;
        }
        public async Task<List<FixtureModel>> GetPredictions()
        {
            var predictTeamId = 6;// "Liverpool";
            
            //var fixtures = await _context.Fixtures.Where(f => f.HomeTeamId == predictTeamId || f.AwayTeamId == predictTeamId).ToListAsync();
            var fmodel =
            from f in _context.Fixtures.Where(f => f.HomeTeamId == predictTeamId || f.AwayTeamId == predictTeamId)
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

            return fmodel.OrderBy(f=>f.Date).ToList();
        }
        public async Task<FixturePredictionModel> GetPredictionsByFixture(int id)
        {

            var fpmodel = new FixturePredictionModel(); var predictions = await _context.Predictions.Where(f => f.Id == id).ToListAsync();
           
            var fmodel =
            from f in _context.Fixtures.Where(f => f.Id == id)
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
            var pmodel = await _context.Predictions.Where(p => p.FixtureId == id).OrderBy(p => p.UserName).ToListAsync();
            foreach (var item in pmodel)
            {
                var umodel = await _context.Users.Where(u => u.UserName == item.UserName).FirstOrDefaultAsync();
                item.UserName = umodel.Name;
            }
            //var hteam = await _context.Teams.Where(t => t.Id == fmodel.HomeTeamId).FirstOrDefaultAsync();
            //fmodel.HomeTeam = hteam.Team;
            //var ateam = await _context.Teams.Where(t => t.Id == fmodel.AwayTeamId).FirstOrDefaultAsync();
            //fmodel.AwayTeam = ateam.Team;
            

            fpmodel.Fixture = fmodel.FirstOrDefault();
            fpmodel.Predictions = pmodel;

            return fpmodel;
        }



        public async Task<PredictionModel> UpdatePredictions(PredictionModel model)
        {
            var viewModel = _context.Predictions.Where(f => f.Id == model.Id).FirstOrDefault();
            if (viewModel == null)
            {
                var pmodel = new PredictionModel() { FixtureId = model.FixtureId, UserName = model.UserName, HomeTeamScore = model.HomeTeamScore, AwayTeamScore = model.AwayTeamScore, UpdatedDate = DateTime.Now };
                _context.Predictions.Add(pmodel);
            }
            else
            {
                viewModel.HomeTeamScore = model.HomeTeamScore;
                viewModel.AwayTeamScore = model.AwayTeamScore;
                viewModel.UpdatedDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();

            return viewModel;
        }

        public async Task<List<PredictionWinnersModel>> GetPredictionWinners(int fixtureId)
        {
            var viewModel = await _context.PredictionWinners.Where(w => w.FixtureId == fixtureId).ToListAsync();
            return viewModel;
        }

        public async Task<string> GetPredictionFixture(int fixtureId)
        {
            var fmodel =
            from f in _context.Fixtures.Where(f => f.Id == fixtureId)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Location= f.Location,
                HomeTeamId= f.HomeTeamId,
                AwayTeamId=f.AwayTeamId,
                HomeTeam=t1.Team,
                AwayTeam=t2.Team,
                Result=f.Result,
                IsLocked=f.IsLocked
            };

           
            return fmodel.FirstOrDefault().HomeTeam + " vs " + fmodel.FirstOrDefault().AwayTeam;
        }

        public async Task<List<PredictionStandingsModel>> GetPredictionStandings()
        {

            var returnModel = await _context.PredictionWinners
                .GroupBy(g => g.UserName)
                .Select(cl => new PredictionStandingsModel
                {
                    Name = cl.First().Name,
                    Username = cl.Key,
                    TotalPoints = cl.Sum(c => c.Point),
                })
                .ToListAsync();


            return returnModel.OrderByDescending(m => m.TotalPoints).ToList();
        }

        public async Task<List<PredictionStandingsModel>> GetMemberPredictionStandings(string userName)
        {

            var returnModel = await _context.PredictionWinners
                .Where(p=>p.UserName == userName)
                .GroupBy(g => g.UserName)
                .Select(cl => new PredictionStandingsModel
                {
                    Name = cl.First().Name,
                    Username = cl.Key,
                    TotalPoints = cl.Sum(c => c.Point),
                })
                .ToListAsync();          

            return returnModel.OrderByDescending(m => m.TotalPoints).ToList();
        }
        public async Task<PredictionModel> GetMyPrediction(int fixtureId, string userName)
        {
            var viewModel = _context.Predictions.Where(f => f.FixtureId == fixtureId && f.UserName == userName).FirstOrDefault();
            if (viewModel == null)
            {
                viewModel = new PredictionModel() { Id = 9999, FixtureId = fixtureId, UserName = userName, HomeTeamScore = null, AwayTeamScore = null };
            }
            return viewModel;
        }
        public async Task<bool> UpdateMyPrediction(PredictionModel predictionModel)
        {
            var ret = false;
            if (predictionModel.Id == 9999)
            {
                var pmodel = new PredictionModel()
                {
                    FixtureId = predictionModel.FixtureId,
                    UserName = predictionModel.UserName,
                    HomeTeamScore = predictionModel.HomeTeamScore,
                    AwayTeamScore = predictionModel.AwayTeamScore,
                    UpdatedDate = DateTime.Now
                };
                _context.Predictions.Add(pmodel);
                ret = true;
            }
            else
            {
                _context.Predictions.Update(predictionModel);
                ret = true;
            }
            await _context.SaveChangesAsync();
            return ret;
        }
    }
}