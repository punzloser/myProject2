using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Catalog.Users
{
    public class LoginValidate : AbstractValidator<LoginRequest>
    {
        public LoginValidate()
        {
            RuleFor(a => a.UserName).NotEmpty().WithMessage("Tài khoản đang trống");
            RuleFor(a => a.Pass).NotEmpty().WithMessage("Mật khẩu đang trống").MinimumLength(6)
                .WithMessage("Mật khẩu trên 6 kí tự");
        }
    }
}
