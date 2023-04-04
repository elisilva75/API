using API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroceriesController : ControllerBase
    {
        private readonly GroceryContext _context;

        public GroceriesController(GroceryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grocery>>> Get()
        {
            return await _context.Groceries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Grocery>> Get(int id)
        {
            var grocery = await _context.Groceries.FindAsync(id);
            if (grocery == null)
            {
                return NotFound();
            }
            return grocery;
        }

        [HttpPost]
        public async Task<ActionResult<Grocery>> Post([FromBody] Grocery grocery)
        {
            _context.Groceries.Add(grocery);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = grocery.Id }, grocery);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Grocery grocery)
        {
            if (id != grocery.Id)
            {
                return BadRequest();
            }

            _context.Entry(grocery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroceryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<Grocery> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var grocery = await _context.Groceries.FindAsync(id);
            if (grocery == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(grocery, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return Ok(grocery);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var grocery = await _context.Groceries.FindAsync(id);
            if (grocery == null)
            {
                return NotFound();
            }

            _context.Groceries.Remove(grocery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroceryExists(int id)
        {
            return _context.Groceries.Any(e => e.Id == id);
        }
    }
}
