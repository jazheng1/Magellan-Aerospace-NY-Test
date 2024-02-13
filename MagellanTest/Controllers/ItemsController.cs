using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagellanTest.Model;

namespace MagellanTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public ItemsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult<Item> CreateItem([FromBody]Item newItem)
        {   
            var itemId = _dbContext.Add(newItem);
            return Ok(new { id = itemId });
        }

        [HttpGet("{id}")]
        public ActionResult<Item> Get(int itemID) {
            var todo = _dbContext.Items.Find(itemID);
            if (todo == null) {
                return NotFound();
            }
            return Ok(todo);
        }

        [HttpGet("{item_name}")]
        public ActionResult<Item> getTotalCost([FromQuery] string item_name) {
            var item = _dbContext.Items.Find(item_name);
            if (item.parent_item != null)
            {   
                // if item is not a parent, return null
                return null;
            }
            else
            {
                return Ok(CalculateTotalCost(item));
            }
        }

        // Recursive function to calculate total cost
        private int CalculateTotalCost(Item item)
        {
            int totalCost = item.cost;

            // Find all child items of the current item
            var childItems = _dbContext.Items.Where(i => i.parent_item == item.id).ToList();

            // Recursively calculate total cost of child items
            foreach (var childItem in childItems)
            {
                totalCost += CalculateTotalCost(childItem);
            }
            return totalCost;
        }
    }
}
