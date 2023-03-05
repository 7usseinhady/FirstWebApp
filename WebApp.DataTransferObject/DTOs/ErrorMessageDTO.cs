
namespace WebApp.DataTransferObjects.DTOs
{
    public partial class ErrorMessageDTO
    {
        public string Id { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

    }
}
