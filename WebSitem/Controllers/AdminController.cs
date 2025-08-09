using Microsoft.AspNetCore.Mvc;
using WebSitem.Context;
using WebSitem.Models;
using WebSitem.Models.ViewModels;
using WebSitem.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using AspNetCoreGeneratedDocument;
using MySqlConnector;


namespace WebSitem.Controllers

{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // bu controller için önbellekleme devre dışı bırakıldı yani logout yaptıktan sonra geri tuşuna basınca admine yönlendirilmiyorsun


    public class AdminController : Controller
    {

        private readonly BlogDbContext _context;
        private readonly UserManager<BlogIdentityUser> _userManager;
        private readonly SignInManager<BlogIdentityUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        public AdminController(BlogDbContext context, UserManager<BlogIdentityUser> userManager, SignInManager<BlogIdentityUser> signInManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }






        // public IActionResult Index()
        // {
        //     DashboardViewModel dashboard = new DashboardViewModel();
        //     MySqlConnection connection = (MySqlConnection)_context.Database.GetDbConnection();
        //     connection.Open();
        //     MySqlCommand command= new MySqlCommand("SELECT count(*) as toplamBlogsayisi from WebSitemDb.Blogs", connection);
        //     MySqlDataReader reader = command.ExecuteReader();
        //     reader.Read();
        //     int count_rows = reader.GetInt32(0);
        //     reader.Close();
        //     connection.Close();
        //     dashboard.TotalBlogCount = count_rows;            

        //     return View(dashboard);
        // }
        public IActionResult Index()
        {
            var dashboard = new DashboardViewModel();
            var totalBlogs = _context.Blogs.Count();
            dashboard.TotalBlogCount = totalBlogs;
            // Tüm blogların ViewCount değerlerini toplayıp, toplam görüntülenme sayısını hesapla.
            var totalViews = _context.Blogs.Select(b => b.ViewCount).Sum();
            // "b => b.ViewCount" ifadesi her bir blog için sadece ViewCount sütununu seçer.
            dashboard.TotalViewCount = totalViews;
            var mostViewedBlog = _context.Blogs.OrderByDescending(b => b.ViewCount).FirstOrDefault();// en yeni tarih en üste eskisi altta olur
            dashboard.MostViewedBlog = mostViewedBlog;
            var latestBlog = _context.Blogs.OrderByDescending(b => b.PublishDate).FirstOrDefault();
            dashboard.LatestBlog = latestBlog;
            var totalComments = _context.Comments.Count();
            dashboard.TotalCommentCount = totalComments;

            var encokyorumalanblog = _context.Blogs.OrderByDescending(b => b.CommentCount).FirstOrDefault();
            dashboard.MostCommentedBlog = encokyorumalanblog;
            var today = DateTime.Now.Date; // Bugünün tarihi    
            var todayCommentCount = _context.Comments
                .Where(c => c.PublishDate.Date == today)
                .Count();
            dashboard.TodayCommentCount = todayCommentCount;

            var totalResources = _context.FileResources.Count();
            dashboard.TotalResourceCount = totalResources;
            // Takip edilen blogların sayısını hesapla
            var totalFollowedBlogs = _context.FollowedBlogs.Count();
            dashboard.TotalFollowedBlogCount = totalFollowedBlogs;

            return View(dashboard);



        }
        public IActionResult Blogs()
        {
            var blogs = _context.Blogs.ToList();
            return View(blogs);
        }

        public IActionResult EditBlog(int id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            return View(blog);
        }

        //blog silme işlemi
        public IActionResult DeleteBlog(int id)
        {

            var blogs = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blogs != null)
            {
                _context.Blogs.Remove(blogs);
                _context.SaveChanges();
            }

            return RedirectToAction("Blogs");
        }


        //blog düzenleme  ve kaydetme işlemi
        [HttpPost]
        public IActionResult EditBlog(Blog model)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == model.Id);
            blog.Name = model.Name;
            blog.Description = model.Description;
            blog.ImageUrl = model.ImageUrl;
            blog.Tags = model.Tags;
            _context.SaveChanges();

            return RedirectToAction("Blogs"); ;

        }
        // blog durumunu değiştirme işlemi
        public IActionResult ToggleStatus(int id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == id);
            if (blog.Status == 1)
            {
                blog.Status = 0;
            }
            else
            {
                blog.Status = 1;
            }
            _context.SaveChanges();

            return RedirectToAction("Blogs");
        }

        public IActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBlog(Blog model)
        {
            model.PublishDate = DateTime.Now;
            model.Status = 1; // Varsayılan olarak blog durumu aktif
            _context.Blogs.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Blogs");
        }
        // public IActionResult Comments()
        // {
        //     var comment = _context.Comments.ToList();
        //     return View(comment);
        // }

        public IActionResult Comments(int? blogId)
        {
            var comments = new List<Comment>();
            if (blogId == null)
            {
                comments = _context.Comments.ToList();
            }
            else
            {
                comments = _context.Comments.Where(c => c.BlogId == blogId).ToList();
            }


            return View(comments);

        }

        public IActionResult DeleteComment(int id)
        {

            var comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Comments");
        }


        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Password == model.RePassword)
            {
                var user = new BlogIdentityUser  //biz bunu Identity sınıfındanda türettiğimiz için bir sürü özellik çıkıyor
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    UserName = model.Email // Kullanıcı adını e-posta olarak ayarlıyoruz

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Kullanıcı başarıyla oluşturulduktan sonra yönlendirme yapabilirsiniz
                    return RedirectToAction("Index");
                }
                else
                {
                    // Hata mesajlarını model state'e ekleyin
                    return View();
                }




            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Blogs"); //önce index daha sonra root




        }

        public IActionResult Feedbacks()
        {
            var feedback = _context.Contacts.ToList();
            return View(feedback);
        }


        public IActionResult FileResources()
        {
            var files = _context.FileResources.ToList();
            return View(files); // Burada @model List<FileResource> ile form sayfam
        }

        public IActionResult DeleteResource(int id)
        {
            var file = _context.FileResources.FirstOrDefault(f => f.Id == id);
            if (file != null)
            {
                // Sunucudan dosyayı da sil (eğer dosya kaydı varsa)
                if (!string.IsNullOrEmpty(file.StoredFileName))
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploadsFolder, file.StoredFileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Veritabanından kaydı sil
                _context.FileResources.Remove(file);
                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "Kaynak başarıyla silindi!";
            return RedirectToAction("FileResources");
        }



        public IActionResult CreateFile()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> CreateFile(FileResourceViewModel model)
        {
            if (model.UploadedFile == null && string.IsNullOrWhiteSpace(model.ExternalLink))
            {
                ModelState.AddModelError("", "Dosya veya harici link giriniz.");
                return View(model);
            }
            string storedFileName = null;
            string originalFileName = null;

            if (model.UploadedFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);


                originalFileName = model.UploadedFile.FileName;
                storedFileName = Guid.NewGuid().ToString() + Path.GetExtension(originalFileName); // benzersiz bir dosya adı oluştur

                var filePath = Path.Combine(uploadsFolder, storedFileName);
                await using var fs = new FileStream(filePath, FileMode.Create);
                await model.UploadedFile.CopyToAsync(fs);
            }
            var fileResource = new FileResource
            {
                Title = model.Title,
                Description = model.Description,
                StoredFileName = storedFileName,
                OriginalFileName = originalFileName,
                ExternalLink = model.ExternalLink,
                UploadedAt = DateTime.UtcNow,
                UploadedByUserId = _userManager.GetUserId(User)
            };
            _context.FileResources.Add(fileResource);
            await _context.SaveChangesAsync();








            // Başarılı bir şekilde dosya kaydedildikten sonra yönlendirme yap
            TempData["SuccessMessage"] = "Dosya başarıyla yüklendi.";

            return RedirectToAction("CreateFile");
        }





       public IActionResult FollowedBlogs()
        {
            var followedBlogs = _context.FollowedBlogs.ToList();
            return View(followedBlogs);
        }



        public IActionResult CreateFollowedBlog()
        {
            return View();
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateFollowedBlog(FollowedBlogCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = new FollowedBlog
            {
                Name = model.Name,
                Url = model.Url,
                AddedDate = DateTime.Now,
                AddedByUserId = _userManager.GetUserId(User),
                AddedByUserName = _userManager.GetUserName(User) // Ekleyen kullanıcının adını al
            };
            _context.FollowedBlogs.Add(entity);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Takip edilen blog başarıyla yüklendi.";
            return RedirectToAction("CreateFollowedBlog");
        }
        
        public IActionResult DeleteFollowedBlog(int id)
        {
            var item = _context.FollowedBlogs.Find(id);
            if (item != null)
            {
                _context.FollowedBlogs.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("FollowedBlogs");
        }


        




    }
}
