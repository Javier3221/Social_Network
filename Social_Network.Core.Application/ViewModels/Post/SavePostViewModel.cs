using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You have to write something before posting")]
        [DataType(DataType.Text)]
        public string PostDescription { get; set; }
        public string ImgUrl { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile PostImage { get; set; }
        public int UserId { get; set; }
    }
}
