using System.ComponentModel.DataAnnotations;

namespace WebSitem.Models.ViewModels
{
    public class FollowedBlogCreateViewModel
    {

        public string Name { get; set; }


        [Url(ErrorMessage = "Geçerli bir URL girin")]
        public string Url { get; set; }
        
        
    }
}
