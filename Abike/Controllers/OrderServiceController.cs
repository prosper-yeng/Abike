using Abike.Data;
using Abike.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Abike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderServiceController : ControllerBase
    {
    private readonly ApplicationDBContext _context; //protects the constructor from modification
       
      public OrderServiceController(ApplicationDBContext context)//Getting the database
      {
        _context = context;
   
      } 

         //Get  All orders 
        [HttpGet]
        [Route("")]
        public IActionResult GetOrderService() // Display all OrderService records
        {
            try
            {
                var orderServices = _context.OrderServices.ToList(); // Fetch all records

                // Check if the collection is empty
                if (orderServices == null || !orderServices.Any())
                {
                    return NotFound("No OrderService records found.");
                }

                return Ok(orderServices); // Return the list of OrderService records
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, "An unexpected error occurred while retrieving the records.");
            }
        }


     //Get order by id
      [HttpGet]
        [Route("{id}")]
        public IActionResult GetOrderServiceById([FromRoute] int id)
        {
            // Validate that the ID is greater than 0
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                // Find the OrderService by ID
                var orderService = _context.OrderServices.Find(id);

                // Check if the OrderService record exists
                if (orderService == null)
                {
                    return NotFound($"OrderService with ID {id} not found.");
                }

                return Ok(orderService); // Return the found orderService record
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, "An unexpected error occurred while retrieving the record.");
            }
        }
        //Search order by phone number, email address, and name
        [HttpGet]
        [Route("search")]
        public IActionResult SearchOrderService([FromQuery] string? email, [FromQuery] string? name, [FromQuery] string? phoneNumber)
        {
            // Check if at least one search parameter is provided
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest("At least one search parameter (email, name, or phone number) must be provided.");
            }

            try
            {
                // Start with the full queryable context
                var query = _context.OrderServices.AsQueryable();

                // Apply filters based on the provided parameters
                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(os => os.Email != null && os.Email.Contains(email));
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(os => os.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    query = query.Where(os => os.PhoneNumber.Contains(phoneNumber));
                }

                // Execute the query and retrieve the results
                var results = query.ToList();

                // Check if any records were found
                if (results == null || !results.Any())
                {
                    return NotFound("No matching OrderService records found.");
                }

                // Return the matching records
                return Ok(results);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(500, $"ArgumentNullException: {ex.ParamName} cannot be null.");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        //Create record
        [HttpPost]
        [Route("")]
        public IActionResult CreateOrderService([FromBody] OrderService orderService)
        {
            // Validate if the due date is not in the past
            if (orderService.ExpectedDueDate.Date < DateTime.Now.Date)
            {
                return BadRequest("Expected due date must be today or in the future.");
            }

            try
            {
                _context.OrderServices.Add(orderService);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetOrderServiceById), new { id = orderService.Id }, orderService);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Update Order 
        // PATCH: api/OrderService/update-by-phone/{phoneNumber}
        [HttpPatch("update-by-phone/{phoneNumber}")]
        public IActionResult UpdateOrderServiceByPhoneNumber(string phoneNumber, [FromBody] JsonPatchDocument<OrderService> patchDoc)
        {
            if (patchDoc == null || patchDoc.Operations.Count == 0)
            {
                return BadRequest("Patch document is null or empty.");
            }

            // Find the order by phone number
            var existingOrderService = _context.OrderServices.FirstOrDefault(os => os.PhoneNumber == phoneNumber);
            if (existingOrderService == null)
            {
                return NotFound($"OrderService with phone number {phoneNumber} not found.");
            }

            try
            {
                // Apply the patch to the found entity and track any validation errors
                patchDoc.ApplyTo(existingOrderService, ModelState);

                // Check if the ModelState is valid after patching
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validate ExpectedDueDate if it was updated
                if (existingOrderService.ExpectedDueDate.Date < DateTime.Now.Date)
                {
                    return BadRequest("Expected due date must be today or in the future.");
                }

                // Save the changes to the database
                _context.SaveChanges();

                return Ok(existingOrderService);
            }
            catch (DbUpdateException ex)
            {
                // Log the specific database error (optional: ex)
                return StatusCode(500, "Error updating the OrderService record.");
            }
            catch (JsonPatchException ex)
            {
                // Handle any patch-specific issues
                return BadRequest($"Error applying patch: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log the exception if necessary (optional: ex)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }



        //Delete order
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteOrderService([FromRoute] int id)
        {
            // Validate that the ID is greater than 0
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            // Find the OrderService by ID
            var orderService = _context.OrderServices.Find(id);
            if (orderService == null)
            {
                return NotFound($"OrderService with ID {id} not found.");
            }

            try
            {
                // Remove the OrderService and save changes
                _context.OrderServices.Remove(orderService);
                _context.SaveChanges();

                return Ok($"OrderService with ID {id} deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions (e.g., foreign key constraint issues)
                return StatusCode(500, "Error deleting the OrderService. It may be referenced by other records.");
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


    }    
}
