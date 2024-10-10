using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS1892.EPLPredictor.Models
{
    public class TeamStatModel
    {
        [Key]
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int? Win { get; set; }
        public int? Draw { get; set; }
        public int? Lost { get; set; }
        public int? GF { get; set; }
        public int? GA { get; set; }
        public int? GD { get; set; }
        public int? Points { get; set; }


    }
}

