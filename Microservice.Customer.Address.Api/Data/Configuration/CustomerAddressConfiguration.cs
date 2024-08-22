using Microservice.Customer.Address.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.Customer.Address.Api.Data.Configuration;

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> modelBuilder)
    {
        modelBuilder
            .Property(b => b.Id)
            .HasDefaultValueSql("NEWID()");

        modelBuilder
            .HasOne(e => e.Country);
    }
}