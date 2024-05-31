using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Ruou.Models
{
    public class ReceiptVM
    {
        public Receipt? receipt { get; set; }
        public List<ReceiptDetail>? receiptdetails { get; set; }
        public List<Receipt>? receipts { get; set; }
        public SelectList? user { get; set; }
        public string? chosenUser{ get; set; }
        public string? SearchString { get; set; }
    }
}
