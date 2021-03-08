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

        /// <summary>
        /// String containing path to database
        /// </summary>
        private string PathToDb { get; set; }
        /// <summary>
        /// Actual database
        /// </summary>
        private LiteDatabase EMDatabase { get; set; }

        public EntertainMeRepository()
        {
            EMDatabase = new LiteDatabase(new MemoryStream());
            InitDatabase();
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

            bool dbWasInitialized = !File.Exists(PathToDb);
            EMDatabase = new LiteDatabase(PathToDb);

            if (dbWasInitialized)
            {
                InitDatabase();
            }
        }

        private void InitDatabase()
        {
            var profileCollection = EMDatabase.GetCollection<EMProfile>(Collections.Profiles);
            _ = profileCollection.Insert(new EMProfile
            {
                UserName = "New User"
            });

            var entertainmentTypeCollection = EMDatabase.GetCollection<EMType>(Collections.EntertainmentTypes);
            _ = entertainmentTypeCollection.Insert(new EMType
            {
                Description = "Movie",
            });
            _ = entertainmentTypeCollection.Insert(new EMType
            {
                Description = "Music",
            });
            _ = entertainmentTypeCollection.Insert(new EMType
            {
                Description = "Book",
            });

            var entertainmentProviderCollection = EMDatabase.GetCollection<EMProvider>(Collections.EntertainmentProviders);
            _ = entertainmentProviderCollection.Insert(new EMProvider
            {
                Description = "None",
            });
            _ = entertainmentProviderCollection.Insert(new EMProvider
            {
                Description = "Vudu",
            });
            _ = entertainmentProviderCollection.Insert(new EMProvider
            {
                Description = "Movies Anywhere",
            });
            _ = entertainmentProviderCollection.Insert(new EMProvider
            {
                Description = "Microsoft",
            });
            _ = entertainmentProviderCollection.Insert(new EMProvider
            {
                Description = "Amazon",
            });
            _ = entertainmentProviderCollection.Insert(new EMProvider
            {
                Description = "Google",
            });
        }

        public EMProfile GetEMProfileByName(string username)
        {
            var col = EMDatabase.GetCollection<EMProfile>(Collections.Profiles);
            var result = col.Query()
                .Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();

            return result;
        }

        public EMProfile SaveEMProfile(EMProfile profile)
        {
            var col = EMDatabase.GetCollection<EMProfile>(Collections.Profiles);
            if (profile.Id == 0)
            {
                profile.Id = col.Insert(profile);
            }
            else
            {
                profile.Updated();
                _ = col.Update(profile);
            }

            return profile;
        }

        public IList<EMType> GetEMTypes()
        {
            var col = EMDatabase.GetCollection<EMType>(Collections.EntertainmentTypes);
            var result = col.Query().ToList();

            return result;
        }

        public EMType GetEMTypeByName(string description)
        {

            var col = EMDatabase.GetCollection<EMType>(Collections.EntertainmentTypes);
            var result = col.Query()
                .Where(x => x.Description.ToLower() == description.ToLower()).FirstOrDefault();

            return result;
        }

        public EMType SaveEMType(EMType entertainmentType)
        {
            var col = EMDatabase.GetCollection<EMType>(Collections.EntertainmentTypes);
            if (entertainmentType.Id == 0)
            {
                entertainmentType.Id = col.Insert(entertainmentType);
            }
            else
            {
                entertainmentType.Updated();
                _ = col.Update(entertainmentType);
            }

            return entertainmentType;
        }

        public IList<EMProvider> GetEMProviders()
        {
            var col = EMDatabase.GetCollection<EMProvider>(Collections.EntertainmentProviders);
            var result = col.Query().ToList();

            return result;
        }

        public EMProvider GetEMProviderByName(string description)
        {

            var col = EMDatabase.GetCollection<EMProvider>(Collections.EntertainmentProviders);
            var result = col.Query()
                .Where(x => x.Description.ToLower() == description.ToLower()).FirstOrDefault();

            return result;
        }

        public EMProvider SaveEMProvider(EMProvider entertainmentProvider)
        {
            var col = EMDatabase.GetCollection<EMProvider>(Collections.EntertainmentProviders);
            if (entertainmentProvider.Id == 0)
            {
                entertainmentProvider.Id = col.Insert(entertainmentProvider);
            }
            else
            {
                entertainmentProvider.Updated();
                _ = col.Update(entertainmentProvider);
            }

            return entertainmentProvider;
        }

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
    }
}
