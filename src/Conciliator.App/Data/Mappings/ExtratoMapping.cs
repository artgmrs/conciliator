using Conciliator.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conciliator.App.Data.Mappings
{
    public class ExtratoMapping : IEntityTypeConfiguration<Extrato>
    {
        public void Configure(EntityTypeBuilder<Extrato> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.TransactionType)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(e => e.DatePosted)
                .IsRequired()
                .HasColumnType("datetime");
            
            builder.Property(e => e.TransactionAmount)
                .IsRequired()
                .HasColumnType("decimal(13, 2)");
            
            builder.Property(e => e.Memo)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Extratos");
        }
    }
}