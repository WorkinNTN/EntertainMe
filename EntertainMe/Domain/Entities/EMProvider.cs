using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Different types of entertainment providers such as Amazon or Vudu
    /// </summary>
    public partial class EMProvider: EMBaseEntity
    {
        /// <summary>
        /// Description of the entertainment provider
        /// </summary>
        public string Description { get; set; }
    }
}
