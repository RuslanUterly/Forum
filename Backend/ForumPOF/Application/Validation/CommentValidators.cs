using Application.DTOs.Comments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation;

public class CommentCreateValidator : AbstractValidator<CommentCreateRequest>
{
    public CommentCreateValidator()
    {
        RuleFor(comment => comment.Content)
            .NotEmpty().WithMessage("Комментарий не может быть пустой");
    }
}

public class CommentUpdateValidator : AbstractValidator<CommentUpdateRequest>
{
    public CommentUpdateValidator()
    {
        RuleFor(comment => comment.Content)
            .NotEmpty().WithMessage("Комментарий не может быть пустой");
    }
}
