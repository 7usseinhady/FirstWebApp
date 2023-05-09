namespace WebApp.SharedKernel.Dtos.Request
{
    public class TermRequestDto
    {
        public int Id { get; set; }
        public string TermTitleAr { get; set; } = default!;
        public string TermTitleEn { get; set; } = default!;
        public string TermBodyAr { get; set; } = default!;
        public string TermBodyEn { get; set; } = default!;
        
    }
}
