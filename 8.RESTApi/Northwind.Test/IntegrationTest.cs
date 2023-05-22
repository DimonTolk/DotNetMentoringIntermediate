using CatalogService.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using CatalogService.Infrastructure;
using MediatR;
using System;
using CatalogService.BLL.Common;


namespace CatalogService.Tests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(CatalogServiceContext));
                        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
                        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
                        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                        services.AddDbContext<CatalogServiceContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            TestClient = appFactory.CreateClient();
        }
    }

}
