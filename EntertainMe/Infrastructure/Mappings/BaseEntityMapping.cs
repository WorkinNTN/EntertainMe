using System;
using System.Collections.Generic;
using System.Text;

using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;

using EntertainMe.Domain.Entities;

namespace EntertainMe.Infrastructure.Mappings
{
    public class BaseEntityMapping : DommelEntityMap<BaseEntity>
    {
        public BaseEntityMapping()
        {
            ToTable("BaseEntity");
            Map(be => be.Id).ToColumn("Id").IsKey().IsIdentity();
            Map(be => be.WhenAdded).ToColumn("WhenAdded");
            Map(be => be.WhenUpdated).ToColumn("WhenUpdated");
            Map(be => be.ProfileId).ToColumn("ProfilId");
            Map(be => be.Profile).Ignore();
        }
    }
}
