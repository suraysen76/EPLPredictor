using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<List<FixtureModel>> GetFixtures(string type)
        {
            var fmodel =
            from f in _context.Fixtures
            .OrderBy(f => f.Date)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            where f.Type == type // && (f.HomeTeamId==6 || f.AwayTeamId==6)
            
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Type= f.Type,
                Location = f.Location,
                HomeTeamId = f.HomeTeamId,
                AwayTeamId = f.AwayTeamId,
                HomeTeam = t1.Team,
                AwayTeam = t2.Team,
                Result = f.Result,
                IsLocked = f.IsLocked
            };
           
            return fmodel.OrderBy(f => f.Date).ThenBy(f => f.HomeTeam).ToList();
        }
        public async Task<FixtureModel> GetFixturesById(int id)
        {
            var fmodel =
            from f in _context.Fixtures
            .OrderBy(f => f.Date)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            where f.Id==id

            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Type = f.Type,
                Location = f.Location,
                HomeTeamId = f.HomeTeamId,
                AwayTeamId = f.AwayTeamId,
                HomeTeam = t1.Team,
                AwayTeam = t2.Team,
                Result = f.Result,
                IsLocked = f.IsLocked
            };
            //var fixture = await _context.Fixtures.Where(f=>f.Id==id).ToListAsync();
            return fmodel.FirstOrDefault();
        }
        public async Task<FixtureModel> UpdateFixtures(FixtureModel model)
        {
            var viewModel = _context.Fixtures.Where(f => f.Id == model.Id).FirstOrDefault();
            viewModel.HomeTeamId = model.HomeTeamId;
            viewModel.HomeTeamId = model.HomeTeamId;
            viewModel.Result= model.Result;
            viewModel.Date= model.Date;
            //viewModel.IsLocked= true;
            await _context.SaveChangesAsync();
            
            return viewModel;
        }

        public async Task<List<FixtureModel>> GetFixtureByTeam(int teamId )
        {           
            var fmodel = await _context.Fixtures.Where(f => f.HomeTeamId == teamId    || f.AwayTeamId ==teamId).ToListAsync();
            return fmodel;
        }

        public IEnumerable<SelectListItem> GetTeamsForDrpdn(string type)
        {

            var teams = _context.Teams
                .Where(x => x.Type == type)
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Team
                                })
                        .OrderBy(o=>o.Text);

            return new SelectList(teams, "Value", "Text");
        }

        public async Task<ErrorViewModel> AddFixture(FixtureModel model)
        {

            var response = new ErrorViewModel();
            
            try
            {
                var exist = _context.Fixtures.Where(x => x.Type == model.Type && x.HomeTeamId == model.HomeTeamId && x.AwayTeamId == model.AwayTeamId).Any();
                if (exist)
                {
                   

                    response.Status = false;
                    response.Message = "Fixture " + model.HomeTeam + " vs "+ model.AwayTeam + " already exist in " + model.Type;
                }
                else
                {

                    //update team names
                    model.HomeTeam = _context.Teams
                .Where(x => x.Id == model.HomeTeamId).FirstOrDefault().Team;
                    model.AwayTeam = _context.Teams
               .Where(x => x.Id == model.AwayTeamId).FirstOrDefault().Team;
                    _context.Fixtures.Add(model);

                    _context.SaveChanges();
                    response.Status = true;
                    response.Message = "Added successfully";
                }
            }
            catch (Exception ex)
            {
                response.Status = true;
                response.Message = "Error :" + ex.ToString();
            }
            return response;
        }

    }
}
