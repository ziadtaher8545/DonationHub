using DonationHub.Application.Dtos;
using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.GetAllCategories
{

    public record GetAllCategoriesQuery() : IRequest<Result<List<CategoryDto>>>
    {
    }
}
