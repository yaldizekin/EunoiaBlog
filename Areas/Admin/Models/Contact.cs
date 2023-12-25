using System.ComponentModel.DataAnnotations;


namespace Blog.Areas.Admin.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}

