using DonationHub.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Features.Authentication.ChangePassword
{
    public record ChangePasswordCommand(string CurrentPassword, string NewPassword, string ConfirmNewPassword, string Email) : IRequest<Result>;
}
