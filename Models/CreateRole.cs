using System.ComponentModel.DataAnnotations;

namespace RGPatients.Models
{
    public class CreateRole
    {
        [Required]
        public string RoleName { get; set; }
    }
}
