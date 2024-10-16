using FluentValidation;

namespace Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> UserName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Имя пользователя обязательно.")
            .MinimumLength(3).WithMessage("Имя пользователя должно содержать как минимум 3 символа.")
            .MaximumLength(20).WithMessage("Имя пользователя не может превышать 20 символов.")
            .Matches(@"^(?!\d)[A-Za-z0-9]+$").WithMessage("Имя пользователя может содержать только буквы и цифры, и не должно начинаться с цифры");
    }

    public static IRuleBuilderOptions<T, string> CategoryName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Категория не может быть пустой")
            .MinimumLength(3).WithMessage("Категория должна содержать как минимум 3 символа")
            .MaximumLength(20).WithMessage("Категория не может превышать 20 символов");
    }

    public static IRuleBuilderOptions<T, string> TagTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Тэг не может быть пустой")
            .MinimumLength(2).WithMessage("Тэг должен содержать как минимум 2 символа")
            .MaximumLength(20).WithMessage("Тэг не может превышать 20 символов");
    }

    public static IRuleBuilderOptions<T, string> TopicTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Тема поста не может быть пустой")
            .MinimumLength(8).WithMessage("Наименование поста должно содержать как минимум 8 символа")
            .MaximumLength(120).WithMessage("Наименование поста не может превышать 120 символов");
    }

    public static IRuleBuilderOptions<T, string> TopicContent<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Описание поста не может быть пустым")
            .MinimumLength(10).WithMessage("Описание поста должно содержать как минимум 10 символа")
            .MaximumLength(3000).WithMessage("Описание поста не может превышать 3000 символов");
    }
}