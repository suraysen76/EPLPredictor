using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Services
{
    public class PredictionService
    {
        private readonly DBCtx _context;
        public PredictionService(DBCtx context)
        {
            _context = context;
        }

        public void UploadPrediction(PredictionModel model)
        {
            _context.Predictions.Add(model);
            _context.SaveChanges();
        }
    }
}
