using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class TeamsViewModel
    {
        
            // Display Attribute will appear in the Html.LabelFor
            [Display(Name = "Team")]
            public int SelectedTeamRoleId { get; set; }
            public IEnumerable<SelectListItem> Teams { get; set; }
        
    }
}
