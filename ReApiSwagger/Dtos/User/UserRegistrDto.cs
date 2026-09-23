using FluentValidation;

namespace ReApiSwagger.Dtos.User
{
    public class UserRegistrDto
    {
        public string NickName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
    public class UserRegistrDtoValidator : AbstractValidator<UserRegistrDto>
    {
        public UserRegistrDtoValidator()
        {
            RuleFor(x => x.NickName).NotEmpty().WithMessage("NickName is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
