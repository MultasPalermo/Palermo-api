//using Entity.Domain.Enums;

//namespace Web.Middleware
//{
//    public class DbContextMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public DbContextMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var header = context.Request.Headers["X-DB-Provider"].ToString();

//            if (!Enum.TryParse<DatabaseType>(header, ignoreCase: true, out var provider))
//                provider = DatabaseType.SqlServer; // por defecto

//            context.Items["DbProvider"] = provider;

//            await _next(context);
//        }
//    }

//}
