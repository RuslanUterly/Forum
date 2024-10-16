using Application.DTOs.Topics;
using FluentValidation;

namespace Application.Validation;

public class TopicCreateValidator : AbstractValidator<TopicCreateRequest>
{
    public TopicCreateValidator()
    {
        RuleFor(topic => topic.Title)
            .TopicTitle();
        RuleFor(topic => topic.Content)
            .TopicContent();
        RuleFor(topic => topic.CategoryName)
            .NotEmpty().WithMessage("Категория не может быть пустой");
    }
}

public class TopicUpdateValidator : AbstractValidator<TopicUpdateRequest>
{
    public TopicUpdateValidator()
    {
        RuleFor(topic => topic.Title)
            .TopicTitle();
        RuleFor(topic => topic.Content)
            .TopicContent();
        RuleFor(topic => topic.CategoryName)
            .NotEmpty().WithMessage("Категория не может быть пустой");
    }
}
