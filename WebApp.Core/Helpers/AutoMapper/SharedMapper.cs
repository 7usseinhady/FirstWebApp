using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Runtime.ExceptionServices;
using WebApp.Core.Interfaces;
using WebApp.DataTransferObjects.Filters.Auth;
using WebApp.SharedKernel.Helpers;

namespace WebApp.Core.Helpers.AutoMapper
{
    public class SharedMapper
    {
        private readonly IServer _server;

        public SharedMapper(IServer server)
        {
            _server = server;
        }

        public string BuildFilePath(string path)
        {
            try
            {
                var baseURL = UriUtils.FormatUri(UriUtils.GetServerAddress(_server, true), false, false, false);
                return UriUtils.BuildUniqueURL(baseURL, path);
            }
            catch(AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return string.Empty;
        }

        public string BuildImagePath(string path)
        {
            try
            {
                path = string.IsNullOrEmpty(path)? "/Images/NoImage.jpg" : path;
                var baseURL = UriUtils.FormatUri(UriUtils.GetServerAddress(_server, true), false, false, false);
                return UriUtils.BuildUniqueURL(baseURL, path);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return string.Empty;
        }

        public string BuildProfileImagePath(string? path)
        {
            try
            {
                path = string.IsNullOrEmpty(path) ? "/Images/NoProfile.png" : path;
                var baseURL = UriUtils.FormatUri(UriUtils.GetServerAddress(_server, true), false, false, false);
                return UriUtils.BuildUniqueURL(baseURL, path);
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return string.Empty;
        }

        public DateTime? ToDateTime(string dateOnly)
        {
            try
            {
                if (!string.IsNullOrEmpty(dateOnly))
                    return DateTime.Parse(dateOnly);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TimeSpan? ToTimeSpan(string timeOnly)
        {
            try
            {
                if (!string.IsNullOrEmpty(timeOnly))
                    return TimeSpan.Parse(timeOnly);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string? ToDateOnly(DateTime? dateTime)
        {
            try
            {
                return dateTime?.ToString("dd-MM-yyyy");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string? ToTimeOnly(TimeSpan? timeSpan)
        {
            try
            {
                return timeSpan?.ToString(@"hh\:mm");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public double ToDouble(decimal number)
        {
            try
            {
                return Decimal.ToDouble(number);
            }
            catch
            {
                return 0;
            }
        }

        public decimal ToDecimal(double number)
        {
            try
            {
                return Convert.ToDecimal(number);
            }
            catch
            {
                return 0;
            }
        }
    }
}
