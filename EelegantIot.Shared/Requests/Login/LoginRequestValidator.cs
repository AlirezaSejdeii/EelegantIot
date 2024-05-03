using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EelegantIot.Shared.Requests.Login
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("نام کاربری را وارد کنید");
            RuleFor(x => x.Password).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("کلمه عبور را وارد کنید");
        }
    }
}
