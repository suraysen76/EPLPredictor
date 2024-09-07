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
            var predictTeam = "Liverpool";
            var team = await _context.Teams.Where(t => t.Team == predictTeam).FirstOrDefaultAsync();
            var fixtures = await _context.Fixtures.Where(f => f.HomeTeam == team.Team || f.AwayTeam == team.Team).ToListAsync();


            return fixtures;
        }
        public async Task<FixturePredictionModel> GetPredictionsByFixture(int id)
        {
            var fpmodel = new FixturePredictionModel(); var predictions = await _context.Predictions.Where(f => f.Id == id).ToListAsync();
            var fmodel = await _context.Fixtures.Where(f => f.Id == id).FirstOrDefaultAsync();
            var pmodel = await _context.Predictions.Where(p => p.FixtureId == id).OrderBy(p => p.UserName).ToListAsync();
            foreach (var item in pmodel)
            {
                var umodel = await _context.Users.Where(u => u.UserName == item.UserName).FirstOrDefaultAsync();
                item.UserName = umodel.Name;
            }

            fpmodel.Fixture = fmodel;
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
            var fmodel = await _context.Fixtures.Where(f => f.Id == fixtureId).FirstOrDefaultAsync();
            return fmodel.HomeTeam + " vs " + fmodel.AwayTeam;
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