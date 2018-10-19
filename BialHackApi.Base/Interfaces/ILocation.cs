namespace BialHackApi.Base.Interfaces
{
    public interface ILocation
    {
        long Id { get; set; }
        string ExternalId { get; set; }
        string Title { get; set; }
        string Address { get; set; }
        decimal Latitude { get; set; }
        decimal Longitude { get; set; }
    }
}
