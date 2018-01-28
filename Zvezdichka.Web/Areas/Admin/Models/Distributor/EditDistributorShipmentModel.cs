using System;
using System.Collections.Generic;
using Zvezdichka.Data.Models.Distributors;
using Zvezdichka.Web.Models.Automapper;

namespace Zvezdichka.Web.Areas.Admin.Models.Distributor
{
    public class EditDistributorShipmentModel : IMapFrom<DistributorShipment>, IHaveCustomMapping
    {
        public int Id { get; set; }
        public string Distributor { get; set; }
        public DateTime Date { get; set; }
        public ICollection<CreateDistributorShipmentProductModel> Products { get; set; }

        public void Configure(AutoMapperProfile config)
        {
            config.CreateMap<DistributorShipment, EditDistributorShipmentModel>()
                .ForMember(x => x.Distributor, m => m.MapFrom(c => c.Distributor.Name));
        }
    }
}