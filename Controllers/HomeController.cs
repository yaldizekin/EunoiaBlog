using Blog.Areas.Admin.Models;
using Blog.Data;
using Blog.Data.Migrations;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context)
		{
			_context = context;
		}
		
		public IActionResult Index()
        {
            return View();
        }
		[Route(template: "blog")]
		public IActionResult Blog()
		{

			return View(_context.Blogs.OrderByDescending(Blog => Blog.CreatedOn).ToList());

		}
		[Route(template:"hakkinda")]
		public IActionResult About()
		{

			return View();

		}

		[Route("{slug}")]

		public IActionResult BlogPost(string slug)
		{
			var post = _context
				.Blogs
				.Where(blog => blog.Slug == slug)
				.FirstOrDefault();

			var count = _context.Blogs.ToList().Find(x => x.Slug == slug);

			if (count != null)
			{

				count.Visits++;
				try
				{
					// Değişiklikleri kaydet
					_context.SaveChanges();
					
				}
				catch (Exception ex)
				{
					// Hata durumunu ele al
					Console.WriteLine($"Hata: {ex.Message}");
				}


				return View(post);
			}


			if (post != null)
			{
				return View(post);
			}

			Response.StatusCode = 404;
			return View("PageNotFound");
		}

        [Route(template: "iletisim")]
        public IActionResult Iletisim()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
        [Route(template: "iletisim")]
        public IActionResult Iletisim([Bind("Id,Name,Email,Phone,Message,CreatedOn")] Contact contact)
		{
			if (ModelState.IsValid)
			{
				_context.Add(contact);
				_context.SaveChanges();
				ViewBag.Msg = "Mesajınız başarıyla iletildi";
				return View("Msg");
			}
			return View(contact);
		}
		public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}