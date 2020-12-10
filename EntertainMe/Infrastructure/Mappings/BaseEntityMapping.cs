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
            Map(be => be.Added).ToColumn("Added");
            Map(be => be.Updated).ToColumn("Updated");
            Map(be => be.ProfileId).ToColumn("ProfilId");
            Map(be => be.Profile).Ignore();
        }
    }
}
