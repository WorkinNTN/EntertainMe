using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    /// <summary>
    /// Entertainment asset
    /// </summary>
    public class EMAsset : EMBaseEntity
    {
        /// <summary>
        /// Profile asset is associated with
        /// </summary>
        public EMProfile EMProfile {get;set;}
        /// <summary>
        /// Title of asset
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Description of the asset
        /// </summary>
        public string Description { get; set; }
    }
}
