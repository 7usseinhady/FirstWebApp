using Newtonsoft.Json.Linq;

namespace WebApp.SharedKernel.Extentions
{
    public static class JObjectExtension
    {
        public static TReturnType GetValueType<TReturnType>(this JObject result, string jTokenKey) where TReturnType : IEquatable<TReturnType>
        {
            if (result.GetValue(jTokenKey) is not null)
                return result.GetValue(jTokenKey).ToObject<TReturnType>();
            else
                return default!;
        }

        //public static bool GetValueBoolean(this JObject result, string jTokenKey)
        //{
        //    if (result.GetValue(jTokenKey) is not null)
        //        return result.GetValue(jTokenKey).ToObject<bool>();
        //    else
        //        return false;
        //}
    }
}
