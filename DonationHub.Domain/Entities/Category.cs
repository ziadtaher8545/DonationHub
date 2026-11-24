using DonationHub.Domain.Common.Results;
using DonationHub.Domain.Interfaces;
using DonationHub.Domain.ValueObjects;

namespace DonationHub.Domain.Entities;

public class Category : BaseEntity, ISoftDeletable
{
    public Name Name { get; private set; }
    public Description? Description { get; private set; }
    public List<Campaign>? Campaigns { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public static Category Create(Name name, Description? description = null)
    {
        return new Category
        {
            Name = name,
            Description = description
        };
    }
    private Category()
    {

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
    public Result<bool> HasAnyCampigns()
    {
        return Campaigns.Any();
    }


}