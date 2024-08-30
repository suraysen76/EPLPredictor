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
            var fixtures = await _context.Fixtures.OrderBy(f=>f.Id).ToListAsync();
            return fixtures;
        }
        public async Task<FixtureModel> GetFixturesById(int id)
        {
            var fixture = await _context.Fixtures.Where(f=>f.Id==id).ToListAsync();
            return fixture.FirstOrDefault();
        }



        public async Task<FixtureModel> UpdateFixtures(FixtureModel model)
        {
            var viewModel = _context.Fixtures.Where(f => f.Id == model.Id).FirstOrDefault();
            viewModel.HomeTeam = model.HomeTeam;
            viewModel.AwayTeam = model.AwayTeam;
            viewModel.Result= model.Result;
            viewModel.IsLocked= true;
            await _context.SaveChangesAsync();
            
            return viewModel;
        }
    }
}
