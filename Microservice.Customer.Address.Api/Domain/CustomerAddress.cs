using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Customer.Address.Api.Domain;

[Table("MSOS_CustomerAddress")]
public class CustomerAddress
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    [MaxLength(50)]
    public string AddressLine1 { get; set; }

    [MaxLength(50)]
    public string AddressLine2 { get; set; }

    [MaxLength(50)]
    public string AddressLine3 { get; set; }

    [MaxLength(50)]
    public string TownCity { get; set; }

    [MaxLength(50)]
    public string County { get; set; }

    [MaxLength(10)]
    public string Postcode { get; set; }

    [ForeignKey("Id")]
    [Required]
    public int? CountryId { get; set; }
    public Country Country { get; set; } 

    [Required]
    public DateTime Created { get; set; } = DateTime.Now;

    [Required]
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
