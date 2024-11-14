using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class UserPredictionModel
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }

        public string? Name { get; set; }
        public FixtureModel? Fixture { get; set; }
        public PredictionModel? Prediction { get; set; }


    }
}
