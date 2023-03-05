using Newtonsoft.Json;

namespace WebApp.DataTransferObjects.DTOs
{
    public class LocationDTO
    {
        public LocationDTO()
        {
        }

        public LocationDTO(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

    }
}
