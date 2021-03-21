using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using LiteDB;
using LiteDB.Engine;

using EntertainMe.Domain.Abstracts;
using EntertainMe.Domain.Entities;
using EntertainMe.Domain.ValueObjects;
using EntertainMe.Infrastructure;

namespace EntertainMe.Infrastructure.Repositories
{
    public partial class EntertainMeRepository : IEntertainMeRepository, IDisposable
    {
        private bool disposedValue;
        private BsonMapper mapper = new BsonMapper();

        /// <summary>
        /// String containing path to database
        /// </summary>
        private string PathToDb { get; set; }
        /// <summary>
        /// Actual database
        /// </summary>
        private LiteDatabase EMDatabase { get; set; }

        #region Database collections
        private ILiteCollection<EMProfile> profileCollection;
        private ILiteCollection<EMType> entertainmentTypeCollection;
        private ILiteCollection<EMProvider> entertainmentProviderCollection;
        private ILiteCollection<EMMedium> entertainmentMediumCollection;
        private ILiteCollection<EMValidTypeMedium> entertainmentValidTypeMediumCollection;
        #endregion

        public EntertainMeRepository()
        {
            EMDatabase = new LiteDatabase(new MemoryStream());
            ConfigureDatabase(true);
        }

        /// <summary>
        /// Initial EntertainMe repository
        /// </summary>
        /// <param name="path">Directory to house database file</param>
        /// <param name="file">Database file name</param>
        public EntertainMeRepository(string path, string file)
        {

            if (!path.EndsWith(@"\"))
            {
                path = path + @"\";
            }
            PathToDb = path + file;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            bool needToInitData = !File.Exists(PathToDb);
            EMDatabase = new LiteDatabase(PathToDb);

            ConfigureDatabase(needToInitData);

        }

        /// <summary>
        /// Configure database
        /// </summary>
        /// <param name="needToInitData">If set to true default data is added</param>
        private void ConfigureDatabase(bool needToInitData)
        {
            #region Initialize/Configure collections
            profileCollection = EMDatabase.GetCollection<EMProfile>(Collections.Profiles);
            entertainmentTypeCollection = EMDatabase.GetCollection<EMType>(Collections.EntertainmentTypes);
            entertainmentProviderCollection = EMDatabase.GetCollection<EMProvider>(Collections.EntertainmentProviders);
            entertainmentMediumCollection = EMDatabase.GetCollection<EMMedium>(Collections.EntertainmentMediums);
            entertainmentValidTypeMediumCollection = EMDatabase.GetCollection<EMValidTypeMedium>(Collections.EntertainmentValidTypesMediums);
            mapper.Entity<EMValidTypeMedium>()
                .DbRef(x => x.EMType, Collections.EntertainmentTypes)
                .DbRef(x => x.EMMedium, Collections.EntertainmentMediums);
            #endregion

            #region Initialize default data sets
            if (needToInitData)
            {
                #region Default profile
                _ = profileCollection.Insert(new EMProfile
                {
                    UserName = "New User"
                });
                #endregion

                #region Default types
                var typeValues = new string[] { "Movie", "Music", "Book" };
                foreach (var value in typeValues)
                {
                    _ = entertainmentTypeCollection.Insert(new EMType
                    {
                        Description = value,
                    });
                }
                #endregion

                #region Default providers
                var providerValues = new string[] { "None", "Vudu", "Movies Anywhere", "Microsoft", "Amazon", "Google" };
                foreach (var value in providerValues)
                {
                    _ = entertainmentProviderCollection.Insert(new EMProvider
                    {
                        Description = value,
                    });
                }
                #endregion

                #region Default mediums
                var mediumValues = new string[] { "CD", "DVD", "Cassette", "Hard Cover", "Soft Cover", "Digital", "Video Cassette" };
                foreach (var value in mediumValues)
                {
                    _ = entertainmentMediumCollection.Insert(new EMMedium
                    {
                        Description = value,
                        PickProvider = (value == "Digital") ? true : false
                    });
                }
                #endregion

                #region Default valid medium/type combinations
                foreach (var medium in entertainmentMediumCollection.FindAll())
                {
                    var typeList = "skip";
                    switch (medium.Description.ToLower())
                    {
                        case "cd":
                        case "cassette":
                            typeList = "music;book";
                            break;
                        case "hard cover":
                        case "soft cover":
                            typeList = "book";
                            break;
                        case "dvd":
                        case "video cassette":
                            typeList = "movie";
                            break;
                        case "digital":
                            typeList = "movie;music;book";
                            break;
                        default:
                            typeList = "skip";
                            break;
                    }

                    if (typeList == "skip")
                    {
                        continue;
                    }
                    foreach (var et in entertainmentTypeCollection.Find(et => typeList.Contains(et.Description.ToLower())))
                    {
                        _ = entertainmentValidTypeMediumCollection.Insert(new EMValidTypeMedium
                        {
                            EMMedium = medium,
                            EMType = et,
                        });
                    }
                }
                #endregion

            }
            #endregion

        }

        public EMProfile GetEMProfileByName(string username)
        {
            var result = profileCollection.FindOne(x => x.UserName.ToLower() == username.ToLower());

            return result;
        }

        public EMProfile SaveEMProfile(EMProfile profile)
        {
            if (profile.Id == 0)
            {
                profile.Id = profileCollection.Insert(profile);
            }
            else
            {
                profile.Updated();
                _ = profileCollection.Update(profile);
            }

            return profile;
        }

        public IList<EMType> GetEMTypes()
        {
            var result = entertainmentTypeCollection.Query().ToList();

            return result;
        }

        public EMType GetEMTypeByName(string description)
        {

            var result = entertainmentTypeCollection.FindOne(x => x.Description.ToLower() == description.ToLower());

            return result;
        }

        public EMType SaveEMType(EMType entertainmentType)
        {
            if (entertainmentType.Id == 0)
            {
                entertainmentType.Id = entertainmentTypeCollection.Insert(entertainmentType);
            }
            else
            {
                entertainmentType.Updated();
                _ = entertainmentTypeCollection.Update(entertainmentType);
            }

            return entertainmentType;
        }

        public IList<EMProvider> GetEMProviders()
        {
            var result = entertainmentProviderCollection.Query().ToList();

            return result;
        }

        public EMProvider GetEMProviderByName(string description)
        {

            var result = entertainmentProviderCollection.FindOne(x => x.Description.ToLower() == description.ToLower());

            return result;
        }

        public EMProvider SaveEMProvider(EMProvider entertainmentProvider)
        {
            if (entertainmentProvider.Id == 0)
            {
                entertainmentProvider.Id = entertainmentProviderCollection.Insert(entertainmentProvider);
            }
            else
            {
                entertainmentProvider.Updated();
                _ = entertainmentProviderCollection.Update(entertainmentProvider);
            }

            return entertainmentProvider;
        }

        public IList<EMMedium> GetEMMediums()
        {
            var result = entertainmentMediumCollection.Query().ToList();

            return result;
        }

        public EMMedium GetEMMediumByName(string description)
        {

            var result = entertainmentMediumCollection.FindOne(x => x.Description.ToLower() == description.ToLower());

            return result;
        }

        public EMMedium SaveEMMedium(EMMedium entertainmentMedium)
        {
            if (entertainmentMedium.Id == 0)
            {
                entertainmentMedium.Id = entertainmentMediumCollection.Insert(entertainmentMedium);
            }
            else
            {
                entertainmentMedium.Updated();
                _ = entertainmentMediumCollection.Update(entertainmentMedium);
            }

            return entertainmentMedium;
        }

        public List<EMValidTypeMedium> GetValidMediumsForType(string typeName)
        {
            var type = entertainmentTypeCollection.FindOne(t => t.Description.ToLower() == typeName.ToLower());
            if (type != null)
            {
                var mediums = GetValidMediumsForType(type.Id);
                return mediums;
            }
            return new List<EMValidTypeMedium>();
        }

        public List<EMValidTypeMedium> GetValidMediumsForType(int typeId)
        {
            var mediums = entertainmentValidTypeMediumCollection
                .Include(valid => valid.EMType)
                .Include(valid => valid.EMMedium)
                .Find(valid => valid.EMType.Id == typeId);

            return mediums.ToList();
        }
        
        public List<EMValidTypeMedium> GetValidMediumsForType(EMType emType)
        {
            var mediums = GetValidMediumsForType(emType.Id);
            return mediums;
        }


        public List<EMValidTypeMedium> GetValidTypesForMedium(string mediumName)
        {
            var medium = entertainmentMediumCollection.FindOne(t => t.Description.ToLower() == mediumName.ToLower());
            if (medium != null)
            {
                var types = GetValidTypesForMedium(medium.Id);
                return types;
            }
            return new List<EMValidTypeMedium>();
        }

        public List<EMValidTypeMedium> GetValidTypesForMedium(int mediumId)
        {
            var mediums = entertainmentValidTypeMediumCollection
                .Include(valid => valid.EMType)
                .Include(valid => valid.EMMedium)
                .Find(valid => valid.EMMedium.Id == mediumId);

            return mediums.ToList();
        }

        public List<EMValidTypeMedium> GetValidTypesForMedium(EMMedium emMedium)
        {
            var mediums = GetValidTypesForMedium(emMedium.Id);
            return mediums;
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    EMDatabase.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~EntertainMeRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
