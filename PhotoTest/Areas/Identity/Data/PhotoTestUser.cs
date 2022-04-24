using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PhotoTest.Models;

namespace PhotoTest.Areas.Identity.Data;

// Add profile data for application users by adding properties to the PhotoTestUser class
public class PhotoTestUser : IdentityUser
{
    [MaxLength(256)]
    public string? FirstName { get; set; }
    [MaxLength(256)]
    public string? LastName { get; set; }
    [Range(10, 150)]
    public int? Age { get; set; }
    [DataType(DataType.Date)]
    public DateTime? Birthday { get; set; }
    [DataType(DataType.MultilineText)]
    public string? Bio { get; set; }

    public List<Post>? Posts { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Favorite>? Favorites { get; set; }

    public IEnumerable<PhotoTestUser>? Followers { get; set; }
    public IEnumerable<PhotoTestUser>? Following { get; set; }
}

