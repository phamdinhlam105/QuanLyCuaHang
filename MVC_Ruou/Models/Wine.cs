using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Ruou.Models
{
    public class Wine
    {
        public int ID { get; set; }

        [Display(Name = "Tên rượu")]
        public string? Name { get; set; }

        [Display(Name = "Loại")]
        public string? CategoryName { get; set; }

        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Giá nhập")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal inputPrice { get; set; }

        [Column(TypeName = "decimal(18,2")]
        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal outputPrice { get; set; }
        [Display(Name = "Số lượng")]
        public int? Amount { get; set; }
        [NotMapped]
        [Display(Name = "Ảnh sản phẩm(jpg, jpeg, png, gif")]
        public IFormFile? ImageFile { get; set; }
    }
}
