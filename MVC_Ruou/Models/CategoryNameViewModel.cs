using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Ruou.Models
{
    public class CategoryNameViewModel
    {
        public List<Wine>? wines { get; set; }
        public SelectList? categoryName { get; set; }
        public string? chosenCategory { get; set; }
        public string? SearchString { get; set; }
    }
}
