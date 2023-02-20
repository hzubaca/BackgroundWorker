using FlowerSpot.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace FlowerSpot.Domain.Entities;
public class User : BaseEntity
{
    [Required]
    [StringLength(20)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(50)]
    public string Email { get; set; }

    public ICollection<Flower> Flowers { get; set; }

    public ICollection<Sighting> Sightings { get; set; }
}
