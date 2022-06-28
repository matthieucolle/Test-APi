using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Dtos
{
    public class UserCreateDto
    {

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Locality { get; set; }
    }
}
