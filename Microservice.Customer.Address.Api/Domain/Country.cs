using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Customer.Address.Api.Domain;

[Table("MSOS_Country")]
public class Country
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }    
}