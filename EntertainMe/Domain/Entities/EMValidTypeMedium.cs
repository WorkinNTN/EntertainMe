using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainMe.Domain.Entities
{
    // What combinations of types and mediums are valid
    public class EMValidTypeMedium
    {
        /// <summary>
        /// Type that is part of the valid combination
        /// </summary>
        public EMType EMType { get; set; }
        /// <summary>
        /// Medium that is part of the valid combination
        /// </summary>
        public EMMedium EMMedium { get; set; }
    }
}
