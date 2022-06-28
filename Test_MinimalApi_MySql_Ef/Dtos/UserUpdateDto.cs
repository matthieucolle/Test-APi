using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Dtos
{
    public class UserUpdateDto
    {
      

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Locality { get; set; }
    }
}
