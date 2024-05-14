using FluentValidation;

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
