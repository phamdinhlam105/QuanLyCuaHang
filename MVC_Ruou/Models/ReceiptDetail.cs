using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Ruou.Models
{
    public class ReceiptDetail
    {
        public int ID { get; set; }

        [Display(Name = "Mã hóa đơn")]
        public string? ReceiptID { get; set; }

        [Display(Name = "Tên rượu")]
        public string? Name { get; set; }

        [Display(Name = "Số lượng")]
        public int? Amount { get; set; }

        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Giá nhập")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal inputPrice { get; set; }

        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal outputPrice { get; set; }
    }
}
