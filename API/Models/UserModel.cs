using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class UserModel: IdentityUser
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public  string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }

}
