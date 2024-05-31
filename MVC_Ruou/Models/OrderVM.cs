using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Ruou.Models
{
    public class OrderVM
    {
        public SelectList? productname { get; set; }
        public List<OrderDetail>? orderdetail { get; set; }
        public int idchosenorder { get; set; }
        public OrderDetail? chosenorder { get; set; }
        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Tổng Giá")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal total { get; set; }
    }
}
