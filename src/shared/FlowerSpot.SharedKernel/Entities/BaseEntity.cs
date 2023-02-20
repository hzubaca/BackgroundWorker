using System.ComponentModel.DataAnnotations;

namespace FlowerSpot.SharedKernel.Entities;
public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
