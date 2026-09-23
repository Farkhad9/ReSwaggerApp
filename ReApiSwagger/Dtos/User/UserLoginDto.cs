using FluentValidation;

namespace ReApiSwagger.Dtos.User
{
    public class UserLoginDto
    {
        public string NickName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.NickName).NotEmpty().WithMessage("NickName is required.").MaximumLength(100).WithMessage("NickName cannot exceed 100 characters.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
