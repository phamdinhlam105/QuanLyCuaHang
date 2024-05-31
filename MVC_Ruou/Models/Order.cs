using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Ruou.Controllers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Ruou.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Tổng Giá")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal total { get; set; }
        [Display(Name="Tên khách")]
        public string customerName { get; set; }
        [Display(Name = "SDT khách")]
        public int customerPhone { get; set; }
        public int status { get; set; }
    }
}
