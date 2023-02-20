using FlowerSpot.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace FlowerSpot.Domain.Entities;

public class Flower : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    public string ImageRef { get; set; }

    [StringLength(250)]
    public string Description { get; set; }

    public int UserId { get; set; }

    public DateTime DateModified { get; set; }

    public User User { get; set; }

    public ICollection<Sighting> Sightings { get; set; }
}
