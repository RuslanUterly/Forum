using Application.DTOs.Users;
using Azure.Core;
using FluentValidation;

namespace Application.Validation;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Email)
            .EmailAddress();
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.");
        RuleFor(user => user.UserName)
            .UserName();
    }
}

public class UserUpdateValidator : AbstractValidator<UserUpdateRequest>
{
    public UserUpdateValidator()
    {
        RuleFor(user => user.Email)
            .EmailAddress();
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.");
        RuleFor(user => user.UserName)
            .UserName();
    }
}

public class ReestablishUserValidator : AbstractValidator<ReestablishUserRequest>
{
    public ReestablishUserValidator()
    {
        RuleFor(user => user.Email)
            .EmailAddress();
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.");
    }
}

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email обязателен.");
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Пароль обязателен.");
    }
}
