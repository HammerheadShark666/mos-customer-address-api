using Microservice.Customer.Address.Api.Data.Configuration;
using Microservice.Customer.Address.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Api.Data.Context;
public class CustomerAddressDbContext(DbContextOptions<CustomerAddressDbContext> options) : DbContext(options)
{
    public DbSet<CustomerAddress> CustomerAddress { get; set; }
    public DbSet<Country> Country { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CustomerAddressConfiguration());

        modelBuilder.Entity<Country>().HasData(DefaultData.GetCountryDefaultData());
        modelBuilder.Entity<CustomerAddress>().HasData(DefaultData.GetCustomerAddressDefaultData());
    }
}