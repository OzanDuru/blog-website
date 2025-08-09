// Program.cs ─ Uygulamanın giriş noktası
// "top-level statements" sayesinde Main() yazmaya gerek yok.

using WebSitem.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebSitem.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

// Web host'u inşa etmeye yarayan yardımcı sınıf.
// args → dotnet run sırasında gelen komut-satırı argümanları.
var builder = WebApplication.CreateBuilder(args);


// Bağlantı bilgisini appsettings.json'dan al
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// DbContext'i bağla (Pomelo kullanıyorsak) blogları , yorumları  vs yönetmek için
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 3, 0)))
);
// Identity için DbContext'i bağla
builder.Services.AddDbContext<BlogIdentityDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 3, 0)))
);

//AUTHENTICATION VE IDENTITY SERVİSLERİNİ EKLE
//giriş yapılmadığı zaman ototmatik Blogs/Index e atacak
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

    // ÖNEMLİ: AddEntityFrameworkStores'a yeni oluşturduğumuz BlogIdentityDbContext'i veriyoruz.
builder.Services.AddIdentity<BlogIdentityUser, BlogIdentityRole>()
    .AddEntityFrameworkStores<BlogIdentityDbContext>() // DİKKAT: Burası BlogIdentityDbContext olacak
    .AddDefaultTokenProviders();





// ───────────── SERVICE KONTEYNIRI ─────────────
// DI (Dependency Injection) container’a servis ekleme yeri.
// MVC (Controller + View) altyapısını ekliyoruz.
// *AddControllersWithViews*  → Model-View-Controller pattern’i.
builder.Services.AddControllersWithViews();

// builder.Build() çağrıldığında:
// 1) DI konteynırı finalize edilir
// 2) WebApplication nesnesi oluşur (runtime pipeline'ı taşıyan obje)
var app = builder.Build();

// ───────────── MIDDLEWARE PIPELINE’I ─────────────
// Ortam (Environment) kontrolü: Development, Staging, Production…
if (!app.Environment.IsDevelopment())
{
    // Prod-tarzı ortamda özel hata sayfası (Error action’ı) kullan.
    app.UseExceptionHandler("/Home/Error");

    // HSTS (HTTP Strict Transport Security) 30 gün → tarayıcıya
    // “bu domaine bir daha HTTP gitme, hep HTTPS kullan” talimatı verir.
    app.UseHsts();
}

// HTTP → HTTPS yönlendirme
app.UseHttpsRedirection();

// wwwroot altındaki statik dosyaları (css, js, img) servis et
app.UseStaticFiles();

// Routing (URL → controller/action eşleştirme) mekanizmasını devreye al
app.UseRouting();

// UseAuthentication, UseAuthorization'dan ÖNCE gelmelidir!
app.UseAuthentication();
app.UseAuthorization();

// “Endpoint-routing” ile varsayılan rota şablonu tanımlanır.
// Örn. /         → HomeController.Index()
//      /Blog/Detay/5 → BlogController.Detay(5)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blogs}/{action=Index}/{id?}");

// Pipeline hazır → dinlemeye başla (Kestrel veya IIS Express)
app.Run();
