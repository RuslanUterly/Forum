using Application.DTOs.Posts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation;

public class PostCreateValidator : AbstractValidator<PostCreateRequest>
{
    public PostCreateValidator()
    {
        RuleFor(post => post.Content)
            .NotEmpty().WithMessage("Пост не может быть пустой");
    }
}

public class PostUpdateValidator : AbstractValidator<PostUpdateRequest>
{
    public PostUpdateValidator()
    {
        RuleFor(post => post.Content)
            .NotEmpty().WithMessage("Пост не может быть пустой");
    }
}


