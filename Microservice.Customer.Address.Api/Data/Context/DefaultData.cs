using Microservice.Customer.Address.Api.Domain;

namespace Microservice.Customer.Address.Api.Data.Context;

public class DefaultData
{
    public static List<Country> GetCountryDefaultData()
    {
        return new List<Country>()
        {
            CreateCountry(1, "England"),
            CreateCountry(2, "Scotland"),
            CreateCountry(3, "Wales"),
            CreateCountry(4, "Northern Ireland") 
        };
    }

    private static Country CreateCountry(int id, string name)
    {
        return new Country { Id = id, Name = name };
    }

    public static List<CustomerAddress> GetCustomerAddressDefaultData()
    {
        return new List<CustomerAddress>()
        {
            CreateCustomerAddress(new Guid("6c84d0a3-0c0c-435f-9ae0-4de09247ee15"), "Intergration_Test", "Intergration_Test", "Intergration_Test", "Intergration_Test", "Intergration_Test", "HD6 TRF", 1),
            CreateCustomerAddress(new Guid("929eaf82-e4fd-4efe-9cae-ce4d7e32d159"), "Intergration_Test2", "Intergration_Test2", "Intergration_Test2", "Intergration_Test2", "Intergration_Test2", "ST4 VFR", 2),
        };
    }

    private static CustomerAddress CreateCustomerAddress(Guid customerId, 
                                                         string addressLine1, string addressLine2, string addressLine3,
                                                         string townCity, string county, string postcode, int countryId)
    {
        return new CustomerAddress
        { 
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            AddressLine3 = addressLine3,
            TownCity = townCity,
            County = county,
            Postcode = postcode,
            CountryId = countryId
        };
    } 
}

