using System;
using System.Collections.Generic;
using System.Text;

using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;

using EntertainMe.Domain.Entities;

namespace EntertainMe.Infrastructure.Mappings
{
    public class EntertainmentTypeMapping : DommelEntityMap<EntertainmentType>
    {
        public EntertainmentTypeMapping()
        {
            ToTable("EntertainmentType");
            Map(et => et.Description).ToColumn("Description");
        }

    }
}
