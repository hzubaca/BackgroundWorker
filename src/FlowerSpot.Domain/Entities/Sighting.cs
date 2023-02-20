using FlowerSpot.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace FlowerSpot.Domain.Entities;
public class Sighting : BaseEntity
{
    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }

    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int FlowerId { get; set; }

    public string ImageRef { get; set; }

    public string? Quote { get; set; }

    public User User { get; set; }

    public Flower Flower { get; set; }
}
