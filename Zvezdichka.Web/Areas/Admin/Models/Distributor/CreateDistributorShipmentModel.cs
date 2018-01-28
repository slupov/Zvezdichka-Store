using System;
using System.Collections.Generic;
using Zvezdichka.Data.Models.Distributors;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Admin.Models.Distributor
{
    public class CreateDistributorShipmentModel : IMapFrom<DistributorShipment>, IHaveCustomMapping
    {
        public string Distributor { get; set; }
        public DateTime Date { get; set; }
        public ICollection<CreateDistributorShipmentProductModel> Products { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config.CreateMap<DistributorShipment, CreateDistributorShipmentModel>()
                .ForMember(x => x.Distributor, m => m.MapFrom(cfg => cfg.Distributor.Name));
        }
    }
}