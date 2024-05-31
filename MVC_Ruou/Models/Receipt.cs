using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC_Ruou.Models
{
    public class Receipt
    {
        public int ID { get; set; }

        [Display(Name ="Mã hóa đơn")]
        public string ReceiptID { get; set; }

        [Display(Name = "Người nhập")]
        public string UserName { get; set; }

        [Display(Name = "Tổng số lượng")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Total { get; set; }
        [Display(Name = "Ngày tạo đơn")]
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }    
    }
}
