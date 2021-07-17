using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Catalog.Users
{
    public class RegisterRequest
    {
        [Display(Name = "Họ")]
        public string FirstName { get; set; }
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Pass { get; set; }
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPass { get; set; }
    }
}
