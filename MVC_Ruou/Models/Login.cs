using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_Ruou.Models
{
    public class Login
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Cần nhập tên đăng nhập")]
        public string username { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Cần nhập mật khẩu")]
        public string password { get; set; }
        
        private int status { get; set; }
        public void Check(Login login)
        {
            if ((login.username == "admin") && (login.password == "admin"))
                status = 1;
            else
                status = 0;
        }
        public int CheckValid()
        {
            if (status == 1)
                return 1;
            else
                return 0;
        }
    }
}
