using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public sealed record UserDataDto(Guid Id, string Email, string UserName, string FullName, IReadOnlyList<string> Roles, string PreferredLanguage = "en");

}
