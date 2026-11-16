using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Categories.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;
}
