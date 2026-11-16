using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{

    public  record IdentityUserDto(
        Guid Id,
        string Email,
        string UserName,
        string FullName,
        IReadOnlyList<string> Roles
    );

}
