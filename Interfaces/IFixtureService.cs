using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Interfaces
{
    public interface IFixtureService
    {
        Task<List<FixtureModel>> GetFixtures();
        
        Task<FixtureModel> GetFixturesById(int id);
        Task<FixtureModel> UpdateFixtures(FixtureModel model);

        Task<List<FixtureModel>> GetFixtureByTeam(string team);
    }
}
