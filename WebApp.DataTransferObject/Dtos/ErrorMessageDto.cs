
namespace WebApp.DataTransferObject.Dtos
{
    public partial class ErrorMessageDto
    {
        public string Id { get; set; } = default!;
        public List<string> Messages { get; set; } = new List<string>();

    }
}
