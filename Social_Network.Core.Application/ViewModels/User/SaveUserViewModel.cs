using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        [Required(ErrorMessage = "Your name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Your last name is required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Your Phone is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string ProfileImgUrl { get; set; }
        [Required(ErrorMessage = "You have to include a profile picture")]
        public IFormFile ProfileImage { get; set; }
        [Required(ErrorMessage = "You have to include your email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }
        [Required(ErrorMessage = "You have to choose a user name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "You have to choose a password")]
        [DataType(DataType.Text)]
        public string Password { get; set; }
        [Required(ErrorMessage = "You have to confirm your password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Text)]
        public string ConfirmPassword { get; set; }
        public bool ActivatedAccount { get; set; }
    }
}
