using System.ComponentModel.DataAnnotations;

namespace ISG.attendance.DTOs.Locations
{
    public class CreateUpdateLocationDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
