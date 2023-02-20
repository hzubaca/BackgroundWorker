namespace FlowerSpot.Domain.Dtos;
public class SightingDto
{    
    public int SightingId { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public string Username { get; set; }

    public string FlowerName { get; set; }

    public string ImageRef { get; set; }

    public int LikesCount { get; set; }

    public string? Quote { get; set; }
}
