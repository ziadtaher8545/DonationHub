using DonationHub.Domain.ValueObjects;

namespace DonationHub.Domain.Entities;

public class Category : BaseEntity
{
    public Name Name { get; private set; }
    public Description? Description { get; private set; }
    public List<Campaign>? Campaigns { get; private set; }

    public static Category Create(Name name, Description? description = null)
    {
        return new Category
        {
            Name = name,
            Description = description
        };
    }
    public static Category Create(Guid id, Name name, Description? description = null)
    {
        return new Category
        {
            Id = id,
            Name = name,
            Description = description
        };
    }


}