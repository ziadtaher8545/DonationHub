using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Application.Interfaces
{
    public interface IDonationDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<IdempotencyRecord> IdempotencyRecords { get; set; }
        public DbSet<ProcessedStripeEvent> ProcessedStripeEvents { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PaymentAttempt> PaymentAttempts { get; set; }
    }
}
