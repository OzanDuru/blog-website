using System;

namespace WebSitem.Models
{
    public class FollowedBlog
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public string Url { get; set; }

        // Eklendiği tarihi tutacak
        public DateTime AddedDate { get; set; }

        // Ekleyen kullanıcıyı tutacak
        public string AddedByUserId { get; set; }
        
        public string AddedByUserName { get; set; }

        
        
    }
}