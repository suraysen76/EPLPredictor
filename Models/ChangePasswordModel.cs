﻿using System.ComponentModel.DataAnnotations;

namespace SS1892.EPLPredictor.Models
{
    public class ChangePasswordModel
    {
            
        [Display(Name = "User Name")]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
