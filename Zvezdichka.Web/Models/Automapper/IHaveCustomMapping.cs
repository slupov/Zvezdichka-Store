using AutoMapper;

namespace Zvezdichka.Web.Models.Automapper
{
    public interface IHaveCustomMapping
    {
        void Configure(IMapperConfigurationExpression config);
    }
}
