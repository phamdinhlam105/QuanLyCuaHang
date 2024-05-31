using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Ruou.Models
{
    public class OrderListVM
    {
        public List<Order>? orders { get; set; }
        public string? SearchString { get; set; }
    }
}
