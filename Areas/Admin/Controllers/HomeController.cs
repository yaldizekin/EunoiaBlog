using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Blog.Areas.Admin.Models;
using Slugify;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Areas.Admin.Controllers
{
	
	[Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
	{

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [Route("admin")]
		public IActionResult Index()
		{


			return View(_context.Blogs.OrderByDescending(post => post.CreatedOn).ToList());
		}

		public IActionResult Blog()
		{


			return View(_context.Blogs.OrderByDescending(post => post.CreatedOn).ToList());
		}




		public IActionResult AddBlog()
		{
			return View();
		}


		[HttpPost]
		public IActionResult AddBlog(EunBlog b)
		{

			if (CheckIfSlugExists(b.Slug))
			{
				ViewBag.Msg = "Aynı slug olduğu için ekleme yapılmadı";
				return View("Msg");
			}
			if (b.Image != null)
			{
				var extension = Path.GetExtension(b.Image.FileName);
				var newimagename = Guid.NewGuid() + extension;
				var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backup", "images", newimagename);
				var stream = new FileStream(location, FileMode.Create);

				//b.Image.CopyTo(stream);
				using (var image = Image.Load(b.Image.OpenReadStream()))
				{
					var newWidth = 800; 

					image.Mutate(x => x
						.Resize(new ResizeOptions
						{
							Size = new Size(newWidth /*newHeight*/),
							Mode = ResizeMode.Max // Bu boyutlandırma modunu isteğinize göre değiştirin
						}));

					image.Save(stream, new JpegEncoder()); // Çıktıyı stream'e kaydet, formatı ayarlayabilirsiniz
				}
				b.ImagePath = newimagename;

			}


			_context.Add(b);
			_context.SaveChanges();

			ViewBag.Msg = "İçerik Eklendi";
			return View("Msg");
		}

		[HttpPost]
		public IActionResult GenerateSlug(string title)
		{
			SlugHelper.Config config = new SlugHelper.Config();
			config.CharacterReplacements.Add("ı", "i");
			SlugHelper helper = new SlugHelper(config);

			string slug = helper.GenerateSlug(title);

			return Content(slug);
		}

		public bool CheckIfSlugExists(string slug)
		{
			if (_context.Blogs.Any(p => p.Slug == slug))
			{
				return true;
			}

			return false;
		}

		[HttpPost]
		public IActionResult CheckSlugExists(string slug)
		{

			if (CheckIfSlugExists(slug))
			{
				return Json(
				   new
				   {
					   exists = true
				   }
				);
			}

			return Json(
				   new
				   {
					   exists = false
				   }
				);
		}


		[Route("/admin/deleteblog/{id}")]
		public IActionResult DeleteBlog(int id)
		{
			_context.Blogs.Remove(
					_context.Blogs.Find(id)
				);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}


        
	
        public IActionResult EditBlog(int id)
		{

			var blog = _context.Blogs.SingleOrDefault(i => i.Id == id);


			return View(blog);
		}

		[HttpPost]
		public IActionResult EditBlog(EunBlog e)
		{

			var existingBlog = _context.Blogs.SingleOrDefault(i => i.Id == e.Id);

           
            if (existingBlog.Image != null)
			{
				
				string KayitDizini = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backup", "images");
				string DosyaAdi = existingBlog.ImagePath;
				string SilinecekResim = Path.Combine(KayitDizini, DosyaAdi);
				System.IO.File.Delete(SilinecekResim);

				
                var newimagename = Guid.NewGuid() + Path.GetExtension(existingBlog.Image.FileName);
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backup", "images", newimagename);
               

				using (var dosyaAkisi = new FileStream(location, FileMode.Create))
				{
					existingBlog.Image.CopyTo(dosyaAkisi);
				}
				existingBlog.ImagePath = newimagename;
			}

			existingBlog.Title = e.Title;
			existingBlog.Summary = e.Summary;
			existingBlog.Content = e.Content;
			existingBlog.Slug = existingBlog.Slug;
			existingBlog.CreatedOn = DateTime.Now;

			_context.Update(existingBlog);
            _context.SaveChanges();
            ViewBag.Msg = "İçerik Güncellendi";
            return View("Msg");



        }



	} 
} 

