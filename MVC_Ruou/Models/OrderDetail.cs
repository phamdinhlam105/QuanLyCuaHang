using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Ruou.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdWine { get; set; }
        [Display(Name ="Tên rượu")]
        public string WineName { get; set; }
        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Giá")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal price { get; set; }
        public int Amount { get; set; }
    }
}
