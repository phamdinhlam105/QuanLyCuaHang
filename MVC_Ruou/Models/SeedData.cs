using Microsoft.EntityFrameworkCore;
using MVC_Ruou.Data;

namespace MVC_Ruou.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVC_RuouContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MVC_RuouContext>>()))
            {
                // Kiểm tra thông tin đã tồn tại hay chưa
                if (context.Wine.Any())
                {
                    return;   // Không thêm nếu đã tồn tại trong DB
                }

                context.Wine.AddRange(
                    new Wine
                    {
                        Name = "Chivas 18",
                        CategoryName = "Whisky",
                        inputPrice = 700000,
                        outputPrice = 1100000
                    },
                    new Wine
                    {
                        Name = "Vodka Gold",
                        CategoryName = "vokda",
                        inputPrice = 1000000,
                        outputPrice = 1300000
                    }
                );
                context.SaveChanges();//lưu dữ liệu
            }
        }
    }
}
