using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{
    public class Club
    {
        public Club()
        {
            this.Players = new List<Player>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "俱乐部名称")]
        public string Name { get; set; }

        public List<Player> Players { get; set; }
    }
}