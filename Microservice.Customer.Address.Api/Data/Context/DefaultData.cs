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
            CreateCustomerAddress(new Guid("aa1dc96f-3be5-41cd-8a1b-207284af3fdd"), "23 Long Shank Road", "Bordly", "Lower Horton", "Horton", "West Yorkshire", "HD6 TRF", 1),
            CreateCustomerAddress(new Guid("af95fb7e-8d97-4892-8da3-5e6e51c54044"), "9 Short Eared Owl Lane", "Coorly", "", "Stirling", "Stirlingshire", "ST4 VFR", 2),
            CreateCustomerAddress(new Guid("55b431ff-693e-4664-8f65-cfd8d0b14b1b"), "1 Sparrow Road", "Halthorpe", "", "Swansea", "West Glamorgan", "SW4 NVD", 3),
            CreateCustomerAddress(new Guid("2385de72-2302-4ced-866a-fa199116ca6e"), "3 Osprey Street", "Mineton", "Manely", "Belfast", "County Antrim", "BF2 PLD", 4),
            CreateCustomerAddress(new Guid("47417642-87d9-4047-ae13-4c721d99ab48"), "66 Seagull Way", "Limestone", "", "Oundle", "Northamptonshire", "PE7 8TY", 1),
            CreateCustomerAddress(new Guid("ff4d5a80-81e3-42e3-8052-92cf5c51e797"), "4 Buzzard Lane", "Needleton", "Harlslon", "Aberdeen", "Aberdeenshire", "AB3 DER", 2),
            CreateCustomerAddress(new Guid("5ff79dfe-c1fa-4dd9-996f-bc96649d6dfc"), "15 Avocet Road", "Frodingly", "", "Cardiff", "South Glamorgan", "CF4 DET", 3),
            CreateCustomerAddress(new Guid("ae55b0d1-ba02-41e1-9efa-9b4d4ac15eec"), "18 Curlew Street", "Bardslow", "", "Derry", "County Londonderry", "DR4 GTY", 4),
            CreateCustomerAddress(new Guid("c95ba8ff-06a1-49d0-bc45-83f89b3ce820"), "21 Golden Eagle Way", "Plorton", "Riddleworth", "Leeds", "West Yorkshire", "LS3 VFR", 1),
            CreateCustomerAddress(new Guid("f07e88ac-53b2-4def-af07-957cbb18523c"), "33 Blackbird Lane", "Reaverson", "", "Easington", "Durham", "PL3 ABF", 1)
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

