using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Areas.Admin.Models
{
	public class EunBlog 
	{
		public int Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Summary { get; set; }
        [Required]

        public string Content { get; set; }
        [Required]

        public string Slug { get; set; }
		public int Visits { get; set; } 
		public string ?ImagePath { get; set; }

        [NotMapped]
		public IFormFile? Image { get; set; } 
		public DateTime CreatedOn { get; set; } = DateTime.Now;
		public IdentityUser? User { get; set; }



	}
}
