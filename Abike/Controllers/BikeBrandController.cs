using Abike.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeBrandController : ControllerBase
    {
        private readonly ApplicationDBContext _context; //protects the constructor from modification
       
      public BikeBrandController(ApplicationDBContext context)//Getting the database
      {
        _context = context;
   
      } 

       //Get  All orders 

      [HttpGet]
       [Route("")]
       //IActionResult is a rapper that simplifies code complexities
       public IActionResult GetBikeBrand() //display all bikeBrand record
       {
        try
        {
            var bikeBrand = _context.BikeBrands;
        if(bikeBrand==null)
        {
            return NotFound();//NotFound is a wrapper to helps save time in writing a bunch of code to indicate result not found
        }

        return Ok(bikeBrand); // $"Reading bikeBrand: {id}";
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return StatusCode(500, "An unexpected error occurred while retrieving ServiceType records.");
        }
        
        }

        //Get order by id

      [HttpGet]
       [Route("{id}")]
       //IActionResult is a rapper that simplifies code complexities
       public IActionResult GetBikeBrandById([FromRoute] int id) //display a bikeBrand record by id
       {
        var bikeBrand = _context.BikeBrands.Find(id);
        if(bikeBrand==null)
        {
            return NotFound();//NotFound is a wrapper to helps save time in writing a bunch of code to indicate result not found
        }

        return Ok(bikeBrand); // $"Reading bikeBrand: {id}";
        }


        // POST: api/BikeBrand
        [HttpPost]
        public IActionResult CreateBikeBrand([FromBody] BikeBrand bikeBrand)
        {
            if (bikeBrand == null)
            {
                return BadRequest("BikeBrand is null");
            }

            try
            {
                _context.BikeBrands.Add(bikeBrand);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                // Log the exception (ex) here if needed
                return StatusCode(500, "Internal server error");
            }

            return CreatedAtAction(nameof(GetBikeBrandById), new { id = bikeBrand.Id }, bikeBrand);
        }


        // Update Order
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateBikeBrand(int id, [FromBody] BikeBrand bikeBrand)
        {
            if (bikeBrand == null || id != bikeBrand.Id)
            {
                return BadRequest("Invalid data or mismatching ID.");
            }

            var existingBikeBrand = _context.BikeBrands.Find(id);
            if (existingBikeBrand == null)
            {
                return NotFound($"BikeBrand with ID {id} not found.");
            }

            try
            {
                // Update the bikeBrand record
                _context.Entry(existingBikeBrand).CurrentValues.SetValues(bikeBrand);
                _context.SaveChanges();
                return Ok(bikeBrand); // Return the updated bikeBrand
            }
            catch (DbUpdateException ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Error updating the BikeBrand record.");
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        //Delete order
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteBikeBrand([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var bikeBrand = _context.BikeBrands.Find(id);
            if (bikeBrand == null)
            {
                return NotFound($"BikeBrand with ID {id} not found.");
            }

            try
            {
                _context.BikeBrands.Remove(bikeBrand);
                _context.SaveChanges();
                return Ok($"BikeBrand with ID {id} deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Log exception if necessary
                return StatusCode(500, "Error deleting the BikeBrand record.");
            }
            catch (Exception ex)
            {
                // Handle general errors
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


    }
}
