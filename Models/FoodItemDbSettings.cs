namespace RipStainAPI.Models;


public class FoodItemDbSettings{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName {get; set; } = null!;
    public string FoodItemsCollection {get; set; } = null!;


}