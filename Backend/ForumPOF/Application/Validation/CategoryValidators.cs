using Application.DTOs.Categories;
using Application.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateRequest>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name)
            .CategoryName();
    }
}

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateRequest>
{
    public CategoryUpdateValidator()
    {
        RuleFor(x => x.Name)
            .CategoryName();
    }
}