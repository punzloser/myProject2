using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

            RuleFor(a => a.Dob)
                .NotEqual(DateTime.Now.Date).WithMessage("Ngày sinh không hợp lệ")
                .NotEmpty().WithMessage("Trống");

            RuleFor(a => a.Email).NotEmpty().Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Không đúng định dạng");

            RuleFor(a => a).Custom((request, context) =>
            {
                if (request.ConfirmPass == null || request.Pass == null) return;
                if (request.Pass.CompareTo(request.ConfirmPass) != 0)
                    context.AddFailure("Nhập lại mật khẩu không khớp");

                //chưa validate mật khẩu phức tạp + user trùng
                //string pattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-])$";
                //if (!Regex.IsMatch(request.Pass, pattern))
                //{
                //    context.AddFailure("Mật khẩu phải gồm chữ số, chữ hoa, thường, và có kí tự đặc biệt");
                //}
            });
        }
    }
}
