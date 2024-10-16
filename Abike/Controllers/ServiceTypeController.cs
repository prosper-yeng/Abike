using Abike.Data;
using Abike.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Abike.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
     private readonly ApplicationDBContext _context; //protects the constructor from modification
       
      public ServiceTypeController(ApplicationDBContext context)//Getting the database
      {
        _context = context;
   
      } 

    //Get  All orders 
     [HttpGet]
    [Route("")]
    public IActionResult GetServiceType() // Display all serviceType records
    {
        try
        {
            var serviceTypes = _context.ServiceTypes.ToList();
            
            if (serviceTypes == null || !serviceTypes.Any())
            {
                return NotFound("No ServiceType records found.");
            }

            return Ok(serviceTypes); // Return the list of serviceTypes
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
    public IActionResult GetServiceTypeById([FromRoute] int id)
    {
        // Validate that the ID is a positive integer
        if (id <= 0)
        {
            return BadRequest("Invalid ID.");
        }

        try
        {
            var serviceType = _context.ServiceTypes.Find(id);
            if (serviceType == null)
            {
                return NotFound($"ServiceType with ID {id} not found.");
            }

            return Ok(serviceType); // Return the found serviceType
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors
            return StatusCode(500, "An unexpected error occurred.");
        }
    }



        //Create endpoint 
        [HttpPost]
        [Route("")]
        public IActionResult CreateServiceType([FromBody] ServiceType serviceType)
        {
            if (serviceType == null)
            {
                return BadRequest("Invalid data. ServiceType object is null.");
            }

            // 'Name' is a unique field, validate against duplicates
            var existingServiceType = _context.ServiceTypes.FirstOrDefault(st => st.Name == serviceType.Name);
            if (existingServiceType != null)
            {
                return Conflict("A service type with the same name already exists.");
            }

            try
            {
                _context.ServiceTypes.Add(serviceType);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetServiceTypeById), new { id = serviceType.Id }, serviceType);
            }
            catch (DbUpdateException ex)
            {
                // Log exception if necessary
                return StatusCode(500, "Error creating the ServiceType record.");
            }
            catch (Exception ex)
            {
                // Handle general errors
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        // Update Order
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateServiceType(int id, [FromBody] ServiceType serviceType)
        {
            if (serviceType == null || id != serviceType.Id)
            {
                return BadRequest("Invalid data. ServiceType object is null or ID mismatch.");
            }

            // Check if the serviceType with the given ID exists
            var existingServiceType = _context.ServiceTypes.Find(id);
            if (existingServiceType == null)
            {
                return NotFound($"ServiceType with ID {id} not found.");
            }

            try
            {
                // Update the existing serviceType with the new values
                _context.Entry(existingServiceType).CurrentValues.SetValues(serviceType);
                _context.SaveChanges();
                return Ok(serviceType); // Return the updated serviceType
            }
            catch (DbUpdateException ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Error updating the ServiceType record.");
            }
            catch (Exception ex)
            {
                // Handle general errors
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Delete order
       [HttpDelete]
       [Route("{id}")]
        public IActionResult DeleteServiceType([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var serviceType = _context.ServiceTypes.Find(id);
            if (serviceType == null)
            {
                return NotFound($"ServiceType with ID {id} not found.");
            }

            try
            {
                _context.ServiceTypes.Remove(serviceType);
                _context.SaveChanges();
                return Ok($"ServiceType with ID {id} deleted successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Log exception if necessary
                return StatusCode(500, "Error deleting the ServiceType record. It might be referenced in other entities.");
            }
            catch (Exception ex)
            {
                // Handle general errors
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        


    }
}
