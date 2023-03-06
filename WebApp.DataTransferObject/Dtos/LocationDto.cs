using Newtonsoft.Json;

namespace WebApp.DataTransferObjects.Dtos
{
    public class LocationDto
    {
        public LocationDto()
        {
        }

        public LocationDto(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

    }
}
