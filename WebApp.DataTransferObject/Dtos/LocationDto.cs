using Newtonsoft.Json;

namespace WebApp.DataTransferObject.Dtos
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

        public double Latitude { get; set; } = default!;

        public double Longitude { get; set; } = default!;

    }
}
