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
            // var itemId = _dbContext.Add(newItem);
            // return Ok(new { id = itemId });
            _dbContext.Items.Add(newItem);
            _dbContext.SaveChanges(); // Save changes to the database to generate the ID

            // Now newItem.Id will be populated with the generated ID
            return Ok(new { id = newItem });
        }


        [HttpGet("{itemID}")]
        public ActionResult<Item> Get(int itemID) {
            var todo = _dbContext.Items.Find(itemID);
            if (todo == null) {
                return NotFound();
            }
            return Ok(todo);
        }

        [HttpGet("totalCost/{item_name}")]
        public async Task<ActionResult<int>> GetTotalCost(string item_name)
        {
            // Check if the item exists and is a top-level item (parent_item is null)
            var item = await _dbContext.Items
                                       .FirstOrDefaultAsync(i => i.item_name == item_name && i.parent_item == null);

            if (item == null)
            {
                return NotFound("Item not found or is not a parent.");
            }

            // Call the PostgreSQL function
            var connection = _dbContext.Database.GetDbConnection();
            try
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Get_Total_Cost(@itemName);";
                    command.CommandType = System.Data.CommandType.Text;
                    var paramName = command.CreateParameter();
                    paramName.ParameterName = "@itemName";
                    paramName.Value = item_name;
                    command.Parameters.Add(paramName);

                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        // Console.WriteLine((int)result);
                        return Ok((int)result);
                    }
                    else
                    {
                        return NotFound("Could not calculate total cost.");
                    }
                }
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}