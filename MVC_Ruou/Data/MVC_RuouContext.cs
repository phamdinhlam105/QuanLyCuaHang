using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC_Ruou.Models;

namespace MVC_Ruou.Data
{
    public class MVC_RuouContext : DbContext
    {
        public MVC_RuouContext (DbContextOptions<MVC_RuouContext> options)
            : base(options)
        {
        }

        public DbSet<MVC_Ruou.Models.Wine> Wine { get; set; } = default!;
        public DbSet<MVC_Ruou.Models.Receipt> Receipt { get; set; } = default!;
        public DbSet<MVC_Ruou.Models.ReceiptDetail> ReceiptDetail { get; set; } = default!;
        public DbSet<MVC_Ruou.Models.Order> Order { get; set; } = default!;
        public DbSet<MVC_Ruou.Models.OrderDetail> OrderDetail { get; set; } = default!;
    }
}
