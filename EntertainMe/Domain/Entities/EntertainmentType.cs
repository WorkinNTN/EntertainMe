using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Different types of entertainment options
    /// </summary>
    public partial class EntertainmentType : BaseEntity
    {
        /// <summary>
        /// Description of the entertainment type
        /// </summary>
        public string Description { get; set; }
    }
}
