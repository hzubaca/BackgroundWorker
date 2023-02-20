namespace FlowerSpot.Application.Features.Commands.AddFlower;
public class AddFlowerRequest
{
    public string Name { get; set; }

    // Assuming this will be a string representation of GUID named image stored on azure Blob
    public string ImageRef { get; set; }

    public string Description { get; set; }
}
