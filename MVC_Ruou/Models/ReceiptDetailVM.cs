using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Ruou.Models
{
    public class ReceiptDetailVM
    {
        public List<ReceiptDetail>? receiptdetails { get; set; }
        public SelectList? productname { get; set; }
        public string? chosenreceipt { get; set; }
        public ReceiptDetail? chosenproduct { get; set; }
    }
}
