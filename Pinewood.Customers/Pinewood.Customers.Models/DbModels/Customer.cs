using Pinewood.Customers.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Pinewood.Customers.Models.DbModels
{
    [ExcludeFromCodeCoverage]
    [Table("Customer")]
    public class Customer: ICustomer, IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
    }
}
