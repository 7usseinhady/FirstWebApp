using Newtonsoft.Json;

namespace WebApp.DataTransferObjects.Dtos
{
    public class RateDto
    {
        [JsonIgnore]
        public object? Id { get; set; }
        public double? TotalRate { get; set; }
        public double? OneRatePercentage { get; set; }
        public double? TwoRatePercentage { get; set; }
        public double? ThreeRatePercentage { get; set; }
        public double? FourRatePercentage { get; set; }
        public double? FiveRatePercentage { get; set; }
    }
}
