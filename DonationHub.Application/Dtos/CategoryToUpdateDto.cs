using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public class CategoryToUpdateDto
    {
        [Required(ErrorMessage = "Name is Required (en and ar ).")]
        public Name Name { get; set; }
        public Description? Description { get; set; }
    }
}
