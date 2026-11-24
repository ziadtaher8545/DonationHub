using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public record UserInfoDto(Guid Id, string Emai, string FullName)
    {
    }
}
