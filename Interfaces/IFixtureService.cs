﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Interfaces
{
    public interface IFixtureService
    {
        Task<List<FixtureModel>> GetFixtures(string type);
        
        Task<FixtureModel> GetFixturesById(int id);
        Task<FixtureModel> UpdateFixtures(FixtureModel model);

        Task<List<FixtureModel>> GetFixtureByTeam(int teamId);

        IEnumerable<SelectListItem> GetTeamsForDrpdn(string type);

        Task<ErrorViewModel> AddFixture(FixtureModel model);
    }
}
