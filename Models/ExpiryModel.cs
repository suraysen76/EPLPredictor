using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class ExpiryModel
    {
        [Key]
        public int Id { get; set; }

        public bool  CanPredict { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
