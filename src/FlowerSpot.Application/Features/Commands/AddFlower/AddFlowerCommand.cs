using MediatR;

namespace FlowerSpot.Application.Features.Commands.AddFlower;
public class AddFlowerCommand : IRequest
{
    public string Name { get; set; }

    // Assuming this will be a string representation of GUID named image stored on azure Blob
    public string ImageRef { get; set; }

    public string Description { get; set; }

    public string Username { get; set; }

    public AddFlowerCommand(string name, string imageRef, string description, string username)
    {
        Name = name;
        ImageRef = imageRef;
        Description = description;
        Username = username;
    }
}
