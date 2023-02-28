﻿using WebApp.SharedKernel.Extensions;
using System.Text.RegularExpressions;

namespace WebApp.SharedKernel.Helpers
{
    public class ObjectUtils
    {

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{12})$").Success;
        }

        public static bool IsSALocalPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(05[0-9]{8})$").Success;
        }
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        // Get distance between 2 points
        public static async Task<double> GetEuclideanDistanceAsync(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        // Get distance between 2 points
        public static async Task<double> GetHarversineDistanceAsync(double x1, double y1, double x2, double y2, DistanceUnit unit)
        {
            // raduis of the earth by miles or kilometers

            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = (y2 - y1).ToRadians();
            var lng = (x2 - x1).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
            Math.Cos(y1.ToRadians()) * Math.Cos(y2.ToRadians()) *
            Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }
    }
}
