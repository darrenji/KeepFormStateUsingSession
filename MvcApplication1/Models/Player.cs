using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "球员名称")]
        public string Name { get; set; }
    }
}