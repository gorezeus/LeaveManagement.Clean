using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg3MzU2ODAwIiwiaWF0IjoiMTc1NTgyMjM3MCIsImFjY291bnRfaWQiOiIwMTk4Y2YyYWE3ZTY3NWU0OTcyZGNkNjljNzQ0MDVhZCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazM3anBjM3dtdGRwcjh2a2FjNng3cWh5Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.M8vJec0JLnQ9NMOrhIHeolhs72qEOjCTPYYi-SnQiJp62fRbR2Mg_wXNDkFniFLLelSdhUyHVVtmwwqj428VoJB1_rV0OQQm511tMvIxFGGBQrGTDDt6AjDS6gGshx4RzE9QBRcbC-0DP8Vc_DusQgf2yIppToV34qAH_4s3DeMTxw0nCAy_2m1wxeWaKLkV6yNe0Ia7esqPI32gpK8T8cX97g9EaGnmIc2sKX9hAS9MWyqfDK52PGgS1fKlfkE7yjpHAt09CHrceFwiSmhHfLHioSjKQyoedCdufavus6HJZlZ7VcFKfxs0I6f66c464WQiT2o8UuRAixY7BcldDw", Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
