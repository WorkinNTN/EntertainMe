using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Base class that all other classes are derived from
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Unique identifier for entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date entity was added
        /// </summary>
        public DateTime Added { get; set; }
        /// <summary>
        /// Date entity was updated
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// Id of profile entity is associated with
        /// </summary>
        public int ProfileId { get; set; }
        /// <summary>
        /// Profile entity is associated with 
        /// </summary>
        public Profile Profile { get; set; }
    }
}
