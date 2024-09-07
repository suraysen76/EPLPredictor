using SS1892.EPLPredictor.Models;

namespace SS1892.EPLPredictor.Utility
{
    public static class UtilDate
    {
        public static string? Name { get; set; }
        public static ExpiryModel CanPredict(DateTime fixtureDate)
        {
            var ret = new ExpiryModel() { CanPredict=false,ExpiryDate=DateTime.Now};
            DateTime currentTime = DateTime.Now;
            DateTime ExpiryTime = fixtureDate.AddMinutes(-5);

            TimeSpan t = ExpiryTime-currentTime;
            var duration = t.TotalSeconds;

            if (duration > 0)
            {
                ret.CanPredict = true;
                ret.ExpiryDate = ExpiryTime;
            }
            return ret;
        }
    }
}
