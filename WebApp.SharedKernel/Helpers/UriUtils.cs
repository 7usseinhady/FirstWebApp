using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace WebApp.SharedKernel.Helpers
{
    public class UriUtils
    {
        public static string GetServerAddress(IServer server, bool isHttps)
        {
            try
            {
                var uri = string.Empty;
                var adresses = server.Features.Get<IServerAddressesFeature>().Addresses.ToList<string>();
                if (adresses is not null && adresses.Count > 0)
                {
                    for (int i = 0; i < adresses.Count(); i++)
                    {
                        if (isHttps)
                        {
                            if (adresses[i].StartsWith("https"))
                            {
                                uri = adresses[i];
                                break;
                            }
                        }
                        else
                        {
                            if (adresses[i].StartsWith("http:"))
                            {
                                uri = adresses[i];
                                break;
                            }
                        }

                    }
                }
                if (!string.IsNullOrEmpty(uri))
                {
                    uri = uri.EndsWith("/") ? uri.Substring(0, uri.Length - 1) : uri;
                    return uri;
                }
                else
                    throw new Exception("can not find server adress");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string FormatUri(string uri, bool withPortNo = true, bool withPath = true, bool withQuery = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(uri))
                {
                    Uri fullUri = null;
                    if (!withPortNo)
                    {
                        fullUri = new Uri(uri);
                        uri = fullUri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped);
                    }
                    if (!withPath)
                    {
                        fullUri = new Uri(uri);
                        uri = fullUri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Path, UriFormat.UriEscaped);
                    }
                    if (!withQuery)
                    {
                        fullUri = new Uri(uri);
                        uri = fullUri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Query, UriFormat.UriEscaped);
                    }
                    uri = uri.EndsWith("/") ? uri.Substring(0, uri.Length - 1) : uri;
                    return uri;
                }
                throw new ArgumentNullException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string BuildUniqueURL(string baseURL, string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(baseURL) && !string.IsNullOrEmpty(path))
                {
                    baseURL = baseURL.EndsWith("/") ? baseURL.Substring(0, baseURL.Length - 1) : baseURL;

                    var uri = path.Replace(@"\", @"/");
                    uri = uri.StartsWith("/") ? uri : $@"/{uri}";
                    uri = $"{baseURL}{uri}";

                    Guid guid = Guid.NewGuid();
                    string guidString = Convert.ToBase64String(guid.ToByteArray());
                    guidString = guidString.Replace("=", "");
                    guidString = guidString.Replace("+", "");
                    uri = uri + "?r=" + guidString;
                    return uri;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
