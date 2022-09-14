using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactPro.Models;

public class AppUser : IdentityUser
{
    [Required]
    [Display(Name ="First Name")]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} amd a max {1} characters in length.", MinimumLength = 2)]
    public string? FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} amd a max {1} characters in length.", MinimumLength = 2)]
    public string? LastName { get; set; }

    [NotMapped]
    public string? FullName { get { return $"{FirstName} {LastName}"; } }

    public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
    public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
}
