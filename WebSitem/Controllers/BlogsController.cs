using Microsoft.AspNetCore.Mvc;
using WebSitem.Context;
using WebSitem.Models;
using WebSitem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebSitem.Identity;
using X.PagedList;

namespace WebSitem.Controllers;

public class BlogsController : Controller
{
    private readonly BlogDbContext _context;
    private readonly UserManager<BlogIdentityUser> _userManager;
    // private readonly SignInManager<BlogIdentityUser> _signInManager;

    public BlogsController(BlogDbContext context, UserManager<BlogIdentityUser> userManager,
        SignInManager<BlogIdentityUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        // _signInManager = signInManager;


    }
    public IActionResult Index()

    {
        int pageSize = 10; // Varsayılan sayfa numarası
        int pageNumber = 1; // Varsayılan sayfa numarası


        var followedBlogs = _context.FollowedBlogs
                        .OrderBy(b => b.Name)
                        .ToList();

        ViewData["FollowedBlogs"] = followedBlogs;

        // var blogs = _context.Blogs.ToList();
        var blogs = _context.Blogs.Where(x => x.Status == 1).ToList();
        return View(blogs);
    }


    public IActionResult BlogContains(string? query)
    {
        // Arama terimi varsa filtrele
        var blogs = string.IsNullOrEmpty(query)
            ? _context.Blogs.Where(x => x.Status == 1).ToList()
            : _context.Blogs
                .Where(x => x.Status == 1 && (x.Name.Contains(query)))
                .ToList();

        return View(blogs);
    }

    public IActionResult ResourceContains(string query)
    {
        // Arama terimi varsa filtrele
        var files = string.IsNullOrEmpty(query)
            ? _context.FileResources.ToList()
            : _context.FileResources
                .Where(x =>
                    (!string.IsNullOrEmpty(x.Title) && x.Title.ToLower().Contains(query.ToLower())) ||
                    (!string.IsNullOrEmpty(x.OriginalFileName) && x.OriginalFileName.ToLower().Contains(query.ToLower()))
                )
                .ToList();

        return View(files);
    }



    // GET: Blogs/Details/5 (Ben bir detay sayfası istiyorum) ve her  sayfa için IctionResult döndürmek gerekiyor  
    public IActionResult Details(int id)
    {
        var followedBlogs = _context.FollowedBlogs
                        .OrderBy(b => b.Name)
                        .ToList();

        ViewData["FollowedBlogs"] = followedBlogs;

        var blog = _context.Blogs.Where(x => x.Id == id).FirstOrDefault();
        blog.ViewCount++;
        _context.SaveChanges(); // Görüntülenme sayısını artırmak için veritabanına kaydet
        var comments = _context.Comments.Where(x => x.BlogId == id).ToList();
        if (blog == null)
        {
            return NotFound(); // Eğer blog bulunamazsa 404 döndür
        }

        // BlogDetailsViewModel viewModel = new BlogDetailsViewModel();
        // viewModel.Blog = blog;
        // viewModel.Comments = comments;
        // viewModel.NewComment = new Comment { BlogId = id }; ayrıca bu şekilde de yapabilirsin
        var viewModel = new BlogDetailsViewModel
        {
            Blog = blog,
            Comments = comments,
            NewComment = new Comment { BlogId = id }
        };
        return View(viewModel);
    }
    public IActionResult CreateComment(Comment model)
    {
        model.PublishDate = DateTime.Now;
        _context.Comments.Add(model);

        var blog = _context.Blogs.FirstOrDefault(x => x.Id == model.BlogId);
        blog.CommentCount++;
        _context.SaveChanges();
        return RedirectToAction("Details", new { id = model.BlogId });

    }

    public IActionResult Support()
    {
        var followedBlogs = _context.FollowedBlogs
                        .OrderBy(b => b.Name)
                        .ToList();

        ViewData["FollowedBlogs"] = followedBlogs;
        // Destek sayfası için gerekli işlemler
        return View();
    }

    public IActionResult Contact()
    {
        var followedBlogs = _context.FollowedBlogs
                        .OrderBy(b => b.Name)
                        .ToList();

        ViewData["FollowedBlogs"] = followedBlogs;
        // İletişim sayfası için gerekli işlemler
        return View();
    }
    public IActionResult CreateContact(Contact model)
    {

        model.PublishDate = DateTime.Now;
        _context.Contacts.Add(model);
        _context.SaveChanges();
        return RedirectToAction("Index");



    }

    public IActionResult About()
    {
        var followedBlogs = _context.FollowedBlogs
                        .OrderBy(b => b.Name)
                        .ToList();

        ViewData["FollowedBlogs"] = followedBlogs;
        // Hakkında sayfası için gerekli işlemler
        return View();
    }

    public IActionResult Resources()

    {
        var followedBlogs = _context.FollowedBlogs
                        .OrderBy(b => b.Name)
                        .ToList();

        ViewData["FollowedBlogs"] = followedBlogs;
        // Kaynaklar sayfası için gerekli işlemler
        var resources = _context.FileResources.ToList();
        return View(resources);
    }
    
    



    
        
    }





