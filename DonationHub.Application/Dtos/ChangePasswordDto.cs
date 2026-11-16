using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmedNewPassword { get; set; } = default!;
    }
}
