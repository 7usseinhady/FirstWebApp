using Newtonsoft.Json.Linq;

namespace WebApp.SharedKernel.Interfaces
{
    public interface IBaseApiConnection
    {
        void SetApiUri(string UriKey);
        Task<JObject> GetAsync(string controller, string action = null!, string id = null!, string queryString = null!);
        Task<JObject> PostAsync(string controller, string action = null!, string id = null!, string queryString = null!, object body = null!);
        Task<JObject> PutAsync(string controller, string action = null!, string id = null!, string queryString = null!, object body = null!);
        Task<JObject> PatchAsync(string controller, string action = null!, string id = null!, string queryString = null!, object body = null!);
        Task<JObject> DeleteAsync(string controller, string action = null!, string id = null!, string queryString = null!, object body = null!);

    }
}
