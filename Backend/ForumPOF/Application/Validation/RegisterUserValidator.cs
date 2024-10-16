using Application.DTOs.Users;
using FluentValidation;

namespace Application.Validation;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Email).EmailAddress();
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.");
        RuleFor(user => user.UserName)
            .NotEmpty().WithMessage("Имя пользователя обязательно.")
            .MinimumLength(3).WithMessage("Имя пользователя должно содержать как минимум 3 символа.")
            .MaximumLength(20).WithMessage("Имя пользователя не может превышать 20 символов.")
            .Matches(@"^(?!\d)[A-Za-z0-9]+$").WithMessage("Имя пользователя может содержать только буквы и цифры, и не должно начинаться с цифры");
    }
}
