using DonationHub.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.CreateCategory
{
    public record CreateCategoryCommand(Name Name,Description Description) : IRequest<Guid>
    {
    }
}
