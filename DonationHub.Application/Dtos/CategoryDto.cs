using DonationHub.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DonationHub.Application.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public Name Name { get; set; }
        public Description? Description { get; set; }
    }
}
