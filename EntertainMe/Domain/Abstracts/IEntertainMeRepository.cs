using System;
using System.Collections.Generic;
using System.Text;

using EntertainMe.Domain.Entities;
using EntertainMe.Domain.ValueObjects;

namespace EntertainMe.Domain.Abstracts
{
    interface IEntertainMeRepository
    {
        /// <summary>
        /// Save a profile
        /// </summary>
        /// <param name="profile">Profile to save</param>
        EMProfile SaveEMProfile(EMProfile newProfile);
        /// <summary>
        /// Retrieve a profile
        /// </summary>
        /// <param name="username">Usename used to retrieve profile</param>
        /// <returns></returns>
        EMProfile GetEMProfileByName(string username);

        /// <summary>
        /// Retrieve all entertainment types
        /// </summary>
        /// <returns></returns>
        IList<EMType> GetEMTypes();
        /// <summary>
        /// Retrieve an entertainment type
        /// </summary>
        /// <param name="description">Description used to retrieve type by</param>
        /// <returns></returns>
        EMType GetEMTypeByName(string description);
        /// <summary>
        /// Save an entertainment type
        /// </summary>
        /// <param name="entertainmentType"></param>
        /// <returns></returns>
        EMType SaveEMType(EMType entertainmentType);

        /// <summary>
        /// Retrieve all providers
        /// </summary>
        /// <returns></returns>
        IList<EMProvider> GetEMProviders();
        /// <summary>
        /// Retrieve a provider
        /// </summary>
        /// <param name="description">Description used to retrieve provider by</param>
        /// <returns></returns>
        EMProvider GetEMProviderByName(string description);
        /// <summary>
        /// Save a provider
        /// </summary>
        /// <param name="entertainmentType"></param>
        /// <returns></returns>
        EMProvider SaveEMProvider(EMProvider entertainmentProvider);
    }
}
