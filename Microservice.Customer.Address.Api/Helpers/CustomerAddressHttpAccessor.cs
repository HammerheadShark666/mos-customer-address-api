using System.Security.Claims;

namespace Microservice.Customer.Address.Api.Helpers;

public class CustomerAddressHttpAccessor(IHttpContextAccessor accessor) : Interfaces.ICustomerAddressHttpAccessor
{
    public Guid CustomerId
    {
        get
        {
            if (accessor == null || accessor.HttpContext == null || accessor.HttpContext.User == null)
                throw new ArgumentNullException("IHttpContextAccessor not found.");

            var customerId = accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return customerId == null ? throw new ArgumentNullException("NameIdentifier not found with customer Id.") : new(customerId);
        }
    }
}