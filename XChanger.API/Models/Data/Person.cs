namespace XChanger.API.Models.Data;

public class Person
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public List<Pet>? Pets { get; set; }
}