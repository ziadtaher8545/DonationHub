using DonationHub.Application.Dtos;
using DonationHub.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(Guid Id, Name Name, Description Description) : IRequest<CategoryToUpdateDto>
    {
    }
}
