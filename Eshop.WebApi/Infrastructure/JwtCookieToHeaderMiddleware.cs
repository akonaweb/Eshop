namespace Eshop.WebApi.Infrastructure
{
    public class JwtCookieToHeaderMiddleware
    {
        private readonly RequestDelegate next;

        public JwtCookieToHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("accessToken", out var token))
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Append("Authorization", $"Bearer {token}");
                }
            }

            await next(context);
        }
    }

}
