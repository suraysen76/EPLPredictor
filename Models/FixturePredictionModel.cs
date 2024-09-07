namespace SS1892.EPLPredictor.Models
{
    public class FixturePredictionModel
    {

        public FixtureModel? Fixture { get; set; }
        //public string ActualResult { get; set; }
        public List<PredictionModel>? Predictions { get; set; }
    }
}
