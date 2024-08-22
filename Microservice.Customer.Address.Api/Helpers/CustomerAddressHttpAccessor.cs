using System.Security.Claims;

namespace Microservice.Customer.Address.Api.Helpers;

public class CustomerAddressHttpAccessor : Interfaces.ICustomerAddressHttpAccessor
{
    private readonly IHttpContextAccessor _accessor;
    public CustomerAddressHttpAccessor(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid CustomerId => new(_accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
}
