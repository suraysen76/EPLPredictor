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
        Task<PredictionsWithStandingsModel> GetMemberPredictionStandings(int userid);
        Task<PredictionModel> GetMyPrediction(int fixtureId, int userid);

        Task<PredictionModel> GetPrediction(int fixtureId);
        Task<bool> UpdateMyPrediction(PredictionModel predictionModel);

        Task<List<UserPredictionModel>> GetMembersPrediction(int fixId);
        Task<bool> SetMembersPrediction(List<UserPredictionModel> upmodel);
    }
}
