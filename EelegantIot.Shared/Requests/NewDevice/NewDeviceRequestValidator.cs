using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EelegantIot.Shared.Requests.NewDevice
{
    public class NewDeviceRequestValidator : AbstractValidator<NewDeviceRequest>
    {
        public NewDeviceRequestValidator()
        {
            RuleFor(x => x.Name).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("نام مستعار را وارد کنید");
            RuleFor(x => x.Pin).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("پین را وارد کنید");
        }
    }
}
