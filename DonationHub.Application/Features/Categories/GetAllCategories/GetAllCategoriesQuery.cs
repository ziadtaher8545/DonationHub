using DonationHub.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.GetAllCategories
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryDto>>
    {
    }
}
