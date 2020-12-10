using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Dapper;
using System.Data.SQLite;

using EntertainMe.Domain.Abstracts;
using EntertainMe.Domain.Entities;
using EntertainMe.Domain.ValueObjects;
using EntertainMe.Infrastructure;

namespace EntertainMe.Infrastructure.Repositories
{
    public partial class EntertainMeRepository : IEntertainMeRepository
    {
        private string PathToDb { get; set; }

        private string _CurrentDBVersion { get; set; }
        public string CurrentDBVersion { get { return _CurrentDBVersion; } }

        public SQLiteConnection db { get; set; }

        private bool _MigrationOccurred { get; set; }
        public bool MigrationOccurred { get { return _MigrationOccurred; } }

        /// <summary>
        /// Initial EntertainMe repository
        /// </summary>
        /// <param name="path">Directory to house database file</param>
        /// <param name="file">Database file name</param>
        /// <param name="hardInit">If a database file already exists, delete it and create a new one</param>
        /// <param name="dbVersion">What version of the database to create</param>
        /// <param name="autoMigrate">Automigrate database if version that exists is older than what the system set database version is</param>
        public EntertainMeRepository(string path, string file, bool hardInit = false, string dbVersion = Constants.DBVersion, bool autoMigrate = false)
        {

            if (!path.EndsWith(@"\"))
            {
                path = path + @"\";
            }
            PathToDb = path + file;
            _CurrentDBVersion = dbVersion;

            bool shouldGoOn = true;
            Migration initMigration = new Migration(PathToDb, _CurrentDBVersion);
            MigrationResults initResults = initMigration.PerformMigration(hardInit);
            if (!initResults.Success)
            {
                shouldGoOn = false;
            }
            else
            {
                _CurrentDBVersion = initResults.Message;
            }

            if (shouldGoOn && autoMigrate)
            {
                _ = MigrateDatabase();
            }

            db = new SQLiteConnection($"Data Source={PathToDb};Version=3;");
        }

        public MigrationResults MigrateDatabase(bool calledFromAutoUpgrade = false)
        {
            var upgradeMigration = new Migration(PathToDb, Constants.DBVersion);
            var result = upgradeMigration.PerformMigration();
            if (result.Success)
            {
                _MigrationOccurred = result.MigrationOccurred;
                _CurrentDBVersion = result.Message;
            }
            return result;


        }
    }
}
