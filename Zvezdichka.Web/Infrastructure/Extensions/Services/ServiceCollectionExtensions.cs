using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zvezdichka.Services;

namespace Zvezdichka.Web.Infrastructure.Extensions.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds user defined data services as "scoped"
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataServices(
            this IServiceCollection services)
        {
            var types = Assembly
                .GetAssembly(typeof(IService))
                .GetTypes();

            var needed = types
                .Where(t =>
                    t.IsClass && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}") &&
                    (!t.IsAbstract && t.Name.Contains("DataService")))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .ToList();

            needed.ForEach(s => services.AddTransient(s.Interface, s.Implementation));

            return services;
        }
    }
}