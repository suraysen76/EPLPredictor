using Microsoft.EntityFrameworkCore;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Services
{
    public class FixtureService : IFixtureService
    {

        private readonly DBCtx _context;

        public FixtureService(DBCtx context)
        {
            _context = context;
        }
        public async Task<List<FixtureModel>> GetFixtures()
        {
            var fmodel =
            from f in _context.Fixtures.OrderBy(f => f.Date)
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
           
            return fmodel.ToList();
        }
        public async Task<FixtureModel> GetFixturesById(int id)
        {
            var fixture = await _context.Fixtures.Where(f=>f.Id==id).ToListAsync();
            return fixture.FirstOrDefault();
        }
        public async Task<FixtureModel> UpdateFixtures(FixtureModel model)
        {
            var viewModel = _context.Fixtures.Where(f => f.Id == model.Id).FirstOrDefault();
            viewModel.HomeTeamId = model.HomeTeamId;
            viewModel.HomeTeamId = model.HomeTeamId;
            viewModel.Result= model.Result;
            viewModel.IsLocked= true;
            await _context.SaveChangesAsync();
            
            return viewModel;
        }

        public async Task<List<FixtureModel>> GetFixtureByTeam(int teamId )
        {           
            var fmodel = await _context.Fixtures.Where(f => f.HomeTeamId == teamId    || f.AwayTeamId ==teamId).ToListAsync();
            return fmodel;
        }
    }
}
