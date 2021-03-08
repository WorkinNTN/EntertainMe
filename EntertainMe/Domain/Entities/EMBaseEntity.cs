using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Base class that all other classes are derived from
    /// </summary>
    public abstract class EMBaseEntity
    {
        /// <summary>
        /// Unique identifier for entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date entity was added
        /// </summary>
        public DateTime WhenAdded { get; set; }
        /// <summary>
        /// Date entity was updated
        /// </summary>
        public DateTime WhenUpdated { get; set; }

        public EMBaseEntity()
        {
            WhenAdded = DateTime.Now;
            WhenUpdated = DateTime.Now;
        }

        public void Updated()
        {
            WhenUpdated = DateTime.Now;
        }
    }
}
