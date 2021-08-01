using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntertainMe.Domain.Entities
{
    public class EMAssetData : EMBaseEntity
    {
        /// <summary>
        /// Asset extra information is associated with
        /// </summary>
        public EMAsset EMAsset { get; set; }
        /// <summary>
        /// Type (Books, Movies, etc.) of this instance of an asset
        /// </summary>
        public EMType EMType { get; set; }
        /// <summary>
        /// Medium (DVD, CD, Digital, etc.) of this instaance of an asset
        /// </summary>
        public EMMedium EMMedium { get; set; }
        /// <summary>
        /// Provider (Vudu, Google, etc.) of this instance of an asset 
        /// </summary>
        public EMProvider EMProvider { get; set; }
        /// <summary>
        /// Year released
        /// </summary>
        public int Year { get; set; }
    }
}
