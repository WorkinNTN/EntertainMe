using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Different types of medium such as digital, cd, dvd etc.
    /// </summary>
    public partial class EMMedium : EMBaseEntity
    {
        /// <summary>
        /// Description of the entertainment type
        /// </summary>
        public string Description { get; set; }
    }
}
