using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Catalog.Users
{
    public class RegisterValidate : AbstractValidator<RegisterRequest>
    {
        public RegisterValidate()
        {
            RuleFor(a => a.FirstName).NotEmpty().WithMessage("Nhập tên").MaximumLength(100).WithMessage("tối đa 100 ký tự");
            RuleFor(a => a.LastName).NotEmpty().WithMessage("Nhập Họ + tên đệm").MaximumLength(100).WithMessage("tối đa 100 ký tự");
            RuleFor(a => a.UserName).NotEmpty().WithMessage("Nhập tên tài khoản");
            RuleFor(a => a.Pass).NotEmpty().WithMessage("Nhập mật khẩu").MinimumLength(6).WithMessage("Tối thiểu 6 kí tự");
            RuleFor(a => a.Dob).NotEqual(DateTime.Now).WithMessage("Ngày sinh không hợp lệ");
            RuleFor(a => a.PhoneNumber).NotEmpty().WithMessage("Nhập SĐT");
            RuleFor(a => a.Email).NotEmpty().Must(IsValid).WithMessage("Không đúng định dạng");

            RuleFor(a => a).Custom((request, context) =>
            {
                if (request.Pass.CompareTo(request.ConfirmPass) != 0)
                    context.AddFailure("Nhập lại mật khẩu không khớp");
            });
        }

        public bool IsValid(string emailAddress)
        {
            var m = new MailAddress(emailAddress);
            if (m != null)
                return true;
            return false;
        }
    }
}
