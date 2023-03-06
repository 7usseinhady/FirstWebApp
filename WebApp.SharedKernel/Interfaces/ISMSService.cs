﻿
namespace WebApp.SharedKernel.Interfaces
{
    public interface ISmsService
    {
        Task SendAsync(string mobileNumber, string body);
        Task SendMultiAsync(List<string> lMobileNumbers, string body);
    }
}
