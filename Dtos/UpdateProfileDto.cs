using System.ComponentModel.DataAnnotations;

namespace Goole_OpenId.Dtos
{
    public class UpdateProfileDto
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? City { get; set; }
    }
}
