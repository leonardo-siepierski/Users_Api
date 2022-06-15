using System;
using System.ComponentModel.DataAnnotations;

namespace usersapi.Models
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? Surname { get; set; }

        [Required]
        public int Age { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
