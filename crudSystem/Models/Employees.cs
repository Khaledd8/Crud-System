// Models/Item.cs
namespace CrudSystem.Models{
public class Employees
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? GradUndergrad { get; set; }
    public string? ImagePath { get; set; } // Path to the uploaded image
}
}