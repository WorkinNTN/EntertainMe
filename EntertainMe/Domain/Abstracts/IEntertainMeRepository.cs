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
        /// <param name="entertainmentProvider"></param>
        /// <returns></returns>
        EMProvider SaveEMProvider(EMProvider entertainmentProvider);

        /// <summary>
        /// Retrieve all mediums
        /// </summary>
        /// <returns></returns>
        IList<EMMedium> GetEMMediums();
        /// <summary>
        /// Retrieve a medium
        /// </summary>
        /// <param name="description">Description used to retrieve medium by</param>
        /// <returns></returns>
        EMMedium GetEMMediumByName(string description);
        /// <summary>
        /// Save a medium
        /// </summary>
        /// <param name="entertainmentMedium"></param>
        /// <returns></returns>
        EMMedium SaveEMMedium(EMMedium entertainmentMedium);

        /// <summary>
        /// Get valid mediums associated with a type
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns></returns>
        List<EMValidTypeMedium> GetValidMediumsForType(string typeName);
        /// <summary>
        /// Get valid mediums associated with a type
        /// </summary>
        /// <param name="typeId">Id of the type</param>
        /// <returns></returns>
        List<EMValidTypeMedium> GetValidMediumsForType(int typeId);
        /// <summary>
        /// Get valid mediums associated with a type
        /// </summary>
        /// <param name="emType">Type object</param>
        /// <returns></returns>
        List<EMValidTypeMedium> GetValidMediumsForType(EMType emType);

        /// <summary>
        /// Get valid types associated with a medium
        /// </summary>
        /// <param name="mediumName">Name of the medium</param>
        /// <returns></returns>
        List<EMValidTypeMedium> GetValidTypesForMedium(string mediumName);
        /// <summary>
        /// Get valid types associated with a medium
        /// </summary>
        /// <param name="mediumId">Id of the medium</param>
        /// <returns></returns>
        List<EMValidTypeMedium> GetValidTypesForMedium(int mediumId);
        /// <summary>
        /// Get valid types associated with a medium
        /// </summary>
        /// <param name="emMedium">Medium object</param>
        /// <returns></returns>
        List<EMValidTypeMedium> GetValidTypesForMedium(EMMedium emMedium);
    }
}
