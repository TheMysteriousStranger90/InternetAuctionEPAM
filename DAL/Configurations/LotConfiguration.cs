using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// LotConfiguration
    /// </summary>
    public class LotConfiguration : IEntityTypeConfiguration<Lot>
    {
        public void Configure(EntityTypeBuilder<Lot> builder)
        {
            builder
                .HasOne(x => x.User)
                .WithMany(x => x.PurchasedLots)
                .HasForeignKey(x => x.UserId);
            builder
                .HasOne(x => x.Trade)
                .WithOne(x => x.Lot)
                .HasForeignKey<Trade>(x => x.LotId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
