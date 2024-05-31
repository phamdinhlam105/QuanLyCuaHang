using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Ruou.Models
{
    public class ReviewOrder
    {
        public Order? order { get; set; }
        public List<OrderDetail>? orderdetails { get; set; }
    }
}
