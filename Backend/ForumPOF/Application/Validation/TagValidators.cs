using Application.DTOs.Tags;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation;

public class TagCreateValidator : AbstractValidator<TagCreateRequest>
{
    public TagCreateValidator()
    {
        RuleFor(tag => tag.Title)
            .TagTitle();
    }
}

public class TagUpdateValidator : AbstractValidator<TagUpdateRequest>
{
    public TagUpdateValidator()
    {
        RuleFor(tag => tag.Title)
            .TagTitle();
    }
}
