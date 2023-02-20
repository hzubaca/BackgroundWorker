using MediatR;

namespace FlowerSpot.Application.Features.Commands.AddSighting;
public class AddSightingCommand : IRequest
{
    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public string Username { get; set; }

    public int FlowerId { get; set; }

    public string ImageRef { get; set; }

    public AddSightingCommand(double longitude, double latitude, string username, int flowerId, string imageRef)
    {
        Longitude = longitude;
        Latitude = latitude;
        Username = username;
        FlowerId = flowerId;
        ImageRef = imageRef;
    }
}
