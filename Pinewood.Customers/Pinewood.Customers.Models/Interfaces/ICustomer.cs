namespace Pinewood.Customers.Models.Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        int Age { get; set; }
    }
}
