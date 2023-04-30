using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendChallenge.Models;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User
{
    // Unique ID for the user
    [Key]
    public int UserId { get; set; }
    
    // First name of the user
    [Required]
    public string FirstName { get; set; }
    
    // Last name of the user
    [Required]
    public string LastName { get; set; }
    
    // How many days the user has worked for their company
    [DefaultValue(0)]
    public int TenureDays { get; set; }
    
    // The company that the user belongs to
    [ForeignKey("Company")]
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; }  
    // SOLA - creates a property named Company of type Company. 
    // SOLA = This property is marked as virtual, which means it can be overridden by a derived class. 

    /* SOLA
    This line creates a navigation property for the User class that represents 
    the Company that the User belongs to. By including this property, you can easily 
    access the related Company object for a User object. Without this property, you would 
    have to manually join the User and Company tables to retrieve this information.

    Additionally, the "virtual" keyword is used to enable lazy loading of the related Company object. 
    This means that the Company object will only be loaded from the database when it is 
    accessed for the first time. If you do not include the "virtual" keyword, then the related 
    Company object will be loaded immediately with the User object, which can be inefficient in 
    certain scenarios.

    */
}