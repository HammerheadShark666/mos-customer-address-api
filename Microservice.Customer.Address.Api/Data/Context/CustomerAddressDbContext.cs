using Microservice.Customer.Address.Api.Data.Configuration;
using Microservice.Customer.Address.Api.Data.Context;
using Microservice.Customer.Address.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Api.Data.Contexts;
public class CustomerAddressDbContext : DbContext
{
    public CustomerAddressDbContext(DbContextOptions<CustomerAddressDbContext> options) : base(options) { }

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