namespace Models;

public class Type
{
    public Category category { get; set; } = new Category();
    public List<Subcategory> subcategories { get; set; } = new List<Subcategory>();
}