using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abike.Model
{
    public class OrderService
    {
        // Primary key as int, auto-incrementing
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Required name with max length of 100 characters
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        // Required phone number with pattern validation
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        // Optional email with pattern validation
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        // Foreign key for BikeBrand with required constraint
        [Required(ErrorMessage = "Bike brand is required")]
        public int BikeBrandId { get; set; }

        // Navigation property for related BikeBrand entity
        [ForeignKey("BikeBrandId")]
        public BikeBrand? BikeBrand { get; set; }

        // Foreign key for TypeOfService with required constraint
        [Required(ErrorMessage = "Type of service is required")]
        public int TypeOfServiceId { get; set; }

        // Navigation property for related TypeOfService entity
        [ForeignKey("ServiceTypeId")]
        public ServiceType? ServiceType { get; set; }

        // Required expected due date in the present or future
        [Required(ErrorMessage = "Expected due date is required")]
        [DataType(DataType.Date)]
        public DateTime ExpectedDueDate 
        { 
            get; 
            set; 
        }

        // Optional description with a max length of 500 characters
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
