﻿
namespace WebApp.DataTransferObjects.Dtos
{
    public partial class ErrorMessageDto
    {
        public string Id { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

    }
}
