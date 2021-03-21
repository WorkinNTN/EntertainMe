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
    public static class EMConstants
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
    }

    /// <summary>
    /// Collections used to house various pieces of data.
    /// </summary>
    public static class Collections
    {
        /// <summary>
        /// Profile data
        /// </summary>
        public const string EntertainmentProfiles = "EntertainmentProfiles";
        /// <summary>
        /// Entertainment type data
        /// </summary>
        public const string EntertainmentTypes = "EntertainmentTypes";
        /// <summary>
        /// Entertainment provider data
        /// </summary>
        public const string EntertainmentProviders = "EntertainmentProviders";
        /// <summary>
        /// Entertainment medium data
        /// </summary>
        public const string EntertainmentMediums = "EntertainmentMediums";
        /// <summary>
        /// Enterainment types/mediums combination that are valid data
        /// </summary>
        public const string EntertainmentValidTypesMediums = "EntertainmentValidTypesMediums";
        /// <summary>
        /// Entertaiment assets
        /// </summary>
        public const string EntertainmentAssets = "EntertainmentAssets";
    }
}
