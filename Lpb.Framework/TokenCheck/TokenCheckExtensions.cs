using Microsoft.AspNetCore.Builder;

namespace TokenCheck
{
    public static class TokenCheckExtensions
    {
        public static IApplicationBuilder UseTokenCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenCheckMiddleware>();
        }
    }
}
