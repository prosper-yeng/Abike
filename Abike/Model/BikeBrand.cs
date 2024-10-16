using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abike;

public class BikeBrand
{[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Changed from long to int

    [Required(ErrorMessage = "Brand is required")]
    [StringLength(100, ErrorMessage = "Brand cannot be longer than 100 characters")]
    public string Brand { get; set; } = string.Empty;
}
