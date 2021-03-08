using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Different types of entertainment options such as books or movies
    /// </summary>
    public partial class EMType : EMBaseEntity
    {
        /// <summary>
        /// Description of the entertainment type
        /// </summary>
        public string Description { get; set; }
    }
}
