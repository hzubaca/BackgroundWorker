namespace FlowerSpot.Application.Features.Commands.AddSighting;
public class AddSightingRequest
{
    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public int FlowerId { get; set; }

    public string ImageRef { get; set; }
}
