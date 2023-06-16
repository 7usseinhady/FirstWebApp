using Microsoft.AspNetCore.Hosting.Server;
using PhoneNumbers;
using System.Runtime.ExceptionServices;
using WebApp.SharedKernel.Enums;
using WebApp.SharedKernel.Helpers;

namespace WebApp.Core.Helpers.AutoMapper
{
    public partial class SharedMapper
    {
        private readonly IServer _server;
        private readonly PhoneNumberUtil _phoneNumberUtil;
        public SharedMapper(IServer server)
        {
            _server = server;
            _phoneNumberUtil = PhoneNumberUtil.GetInstance();
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

        public string ToInternationalPhoneNumber(string? phoneNumber, Country? country = null)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                    return string.Empty;

                string? region = country is null ? null : country.ToString();
                PhoneNumber parsedPhoneNumber = _phoneNumberUtil.Parse(phoneNumber, region);

                if (!_phoneNumberUtil.IsValidNumber(parsedPhoneNumber))
                    throw new InvalidOperationException("Invalid phone number");

                return _phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.INTERNATIONAL);
            }
            catch
            {
                throw new InvalidOperationException("Invalid phone number");
            }
        }

        public string ToNationalPhoneNumber(string? phoneNumber, Country? country = null)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                    return string.Empty;

                string? region = country is null ? null : country.ToString();
                PhoneNumber parsedPhoneNumber = _phoneNumberUtil.Parse(phoneNumber, region);

                if (!_phoneNumberUtil.IsValidNumber(parsedPhoneNumber))
                    throw new InvalidOperationException("Invalid phone number");

                return _phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.NATIONAL);
            }
            catch
            {
                throw new InvalidOperationException("Invalid phone number");
            }
        }

    }

    // Static Methods
    public partial class SharedMapper
    {
        public static DateTime? ToDateTime(string dateOnly)
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

        public static TimeSpan? ToTimeSpan(string timeOnly)
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

        public static string? ToDateOnly(DateTime? dateTime)
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

        public static string? ToTimeOnly(TimeSpan? timeSpan)
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

        public static double ToDouble(decimal number)
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

        public static decimal ToDecimal(double number)
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
