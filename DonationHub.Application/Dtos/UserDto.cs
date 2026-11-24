using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AcessToken { get; set; }
        public string RefershToken { get; set; }
    }
}
