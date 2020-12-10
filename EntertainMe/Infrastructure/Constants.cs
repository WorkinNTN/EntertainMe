using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;

namespace EntertainMe.Infrastructure
{
    /// <summary>
    /// Infrastructure related constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Path for EntertainMe local storage 
        /// </summary>
        public static string EntertainMePath = Path
            .Combine(Environment.GetFolderPath(SpecialFolder.LocalApplicationData, SpecialFolderOption.DoNotVerify), @"REDWare\EntertainMe\");
        /// <summary>
        /// Name to use for EntertainMe local storage file
        /// </summary>
        public const string EntertainMeDB = "EntertainMe.db";
        /// <summary>
        /// Latest version of database file
        /// </summary>
        public const string DBVersion = "00.03";
    }
}
