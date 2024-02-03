using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Models
{
    public class BookItem
    {
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
