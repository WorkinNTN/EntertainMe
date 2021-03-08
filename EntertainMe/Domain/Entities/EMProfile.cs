using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Information about a user that owns a grouping of entertainment options
    /// </summary>
    public partial class EMProfile : EMBaseEntity
    {
        /// <summary>
        /// Name associated with a profile
        /// </summary>
        public string UserName { get; set; }

        public EMProfile()
        {
        }
    }
}
