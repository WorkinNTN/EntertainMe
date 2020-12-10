using System;
using System.Collections.Generic;
using System.Text;

using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;

using EntertainMe.Domain.Entities;

namespace EntertainMe.Infrastructure.Mappings
{
    public class ProfileMapping : DommelEntityMap<Profile>
    {
        public ProfileMapping()
        {
            ToTable("Profile");
            Map(p => p.ProfileId).ToColumn("ProfileId").IsKey();
            Map(p => p.UserName).ToColumn("UserName");
        }
    }
}
