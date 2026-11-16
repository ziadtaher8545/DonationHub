using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Admin.CreateStaff
{
    public record CreateStaffCommand(string Email, string Role) : IRequest<bool>
    {
    }
}
