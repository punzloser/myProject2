using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace ViewModel.Catalog.Users
{
    public class RegisterValidate : AbstractValidator<RegisterRequest>
    {
        public RegisterValidate()
        {
            RuleFor(a => a.FirstName).NotEmpty().WithMessage("Nhập tên").MaximumLength(100).WithMessage("tối đa 100 ký tự");
            RuleFor(a => a.LastName).NotEmpty().WithMessage("Nhập Họ + tên đệm").MaximumLength(100).WithMessage("tối đa 100 ký tự");
            RuleFor(a => a.UserName).NotEmpty().WithMessage("Nhập tên tài khoản");

            RuleFor(a => a.Dob)
                .NotEqual(DateTime.Now.Date).WithMessage("Ngày sinh không hợp lệ")
                .NotEmpty().WithMessage("Trống");

            RuleFor(a => a).Custom((request, result) =>
            {
                var x = DateTime.Now.Year - request.Dob.Year;
                if (x >= 118)
                    result.AddFailure("Dob", "Tuổi bạn quá là ảo, người giữ kỉ lục hiện tại chỉ 118 tuổi");
                if (x <= 0)
                    result.AddFailure("Dob", "Tuổi phải lớn hơn 0");
            });

            RuleFor(a => a.Email).NotEmpty()
                .WithMessage("Email trống")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Không đúng định dạng");

            RuleFor(a => a).Custom((request, context) =>
            {
                if (request.ConfirmPass == null || request.Pass == null) return;
                if (request.Pass.CompareTo(request.ConfirmPass) != 0)
                    context.AddFailure("Nhập lại mật khẩu không khớp");

                var check = Regex.IsMatch(request.Pass.ToString(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$");
                if (!check)
                {
                    context.AddFailure("Pass", "Mật khẩu tối thiểu 8 kí tự phải gồm chữ số, chữ hoa, thường, và có kí tự đặc biệt");
                }
            });
        }
    }
}
