using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipStainAPI.Models;
using RipStainAPI.Services;
 
[Authorize]
[ApiController]
[Route("[controller]")]
public class FoodItemController : ControllerBase {
    private readonly FoodItemService _fooditemService;

    public FoodItemController(FoodItemService foodItemService){
        _fooditemService = foodItemService;
    }

    [HttpGet]
    public async Task<List<FoodItem>> Get() =>
        await _fooditemService.GetAsync();
}


