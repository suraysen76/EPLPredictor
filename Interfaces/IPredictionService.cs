using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Interfaces
{
    public interface IPredictionService
    {
        Task<List<FixtureModel>> GetPredictions();
        Task<FixturePredictionModel> GetPredictionsByFixture(int id);
        Task<PredictionModel> UpdatePredictions(PredictionModel model);

        Task<List<PredictionWinnersModel>> GetPredictionWinners(int fixtureId);

        Task<string> GetPredictionFixture(int fixtureId);

        Task<List<PredictionStandingsModel>> GetPredictionStandings();
        Task<PredictionModel> GetMyPrediction(int fixtureId, string userName);
        Task<bool> UpdateMyPrediction(PredictionModel predictionModel);
    }
}
