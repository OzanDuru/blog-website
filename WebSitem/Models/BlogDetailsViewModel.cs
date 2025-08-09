namespace WebSitem.Models
{
    public class BlogDetailsViewModel
    {
        public Blog Blog { get; set; }                     // Blog detayları için
        public List<Comment> Comments { get; set; }        // Bloga ait yorumlar
        public Comment NewComment { get; set; }            // Kullanıcının ekleyeceği yorum (form verisi)
    }
}
