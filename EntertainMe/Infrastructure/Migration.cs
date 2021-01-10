using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using EntertainMe.Domain.Entities;
using EntertainMe.Domain.ValueObjects;
using EntertainMe.Infrastructure.Mappings;

namespace EntertainMe.Infrastructure
{
    public partial class Migration
    {
        /// <summary>
        /// Full path to the database file
        /// </summary>
        private string FullPathToDB { get; set; }
        /// <summary>
        /// What version of the database to migrate to
        /// </summary>
        private string VersionToMigateTo { get; set; }

        /// <summary>
        /// Initialize migration object
        /// </summary>
        public Migration()
        { 
        }

        /// <summary>
        /// Initialize migration object
        /// </summary>
        /// <param name="fullPathToDB">Full path to database file</param>
        /// <param name="versionToMigrateTo">Version to migrate to</param>
        public Migration(string fullPathToDB, string versionToMigrateTo)
        {
            FullPathToDB = fullPathToDB;
            VersionToMigateTo = versionToMigrateTo;
        }

        /// <summary>
        /// Perform the migration.  Parameter values are supplied, they override what the object was initialized with
        /// </summary>
        /// <param name="fullPathToDB">Full path to database file</param>
        /// <param name="versionToMigrateTo">Version ot migrate to</param>
        public MigrationResults PerformMigration(bool init = false, string fullPathToDB = null, string versionToMigrateTo = null)
        {
            var results = new MigrationResults
            {
                Success = true,
                MigrationOccurred = false,
            };

            if (string.IsNullOrEmpty(fullPathToDB))
            {
                fullPathToDB = FullPathToDB;
            }
            if (string.IsNullOrEmpty(versionToMigrateTo))
            {
                versionToMigrateTo = VersionToMigateTo;
            }

            if (string.IsNullOrEmpty(fullPathToDB) || string.IsNullOrEmpty(versionToMigrateTo))
            {
                results.Success = false;
                results.MigrationOccurred = false;
                results.Message = "Path to the database file and/or a version to migrate to was not supplied.";
            }

            Logging logging = new Logging();

            if (results.Success)
            {
                logging.SetLoggingInfo(new FileInfo(fullPathToDB).Directory.FullName, "migration");
                logging.DateStamp = true;
                try
                {
                    if (File.Exists(fullPathToDB) && init)
                    {
                        File.Delete(fullPathToDB);
                        logging.Log("Database found and deleted because of init state");
                    }
                    if (!File.Exists(fullPathToDB))
                    {
                        Directory.CreateDirectory(new FileInfo(fullPathToDB).Directory.FullName);
                        SQLiteConnection.CreateFile(fullPathToDB);
                        init = true;
                        logging.Log("Database created.");
                    }
                }
                catch (Exception ex)
                {
                    results.Success = false;
                    results.MigrationOccurred = false;
                    results.Message = ex.Message;
                }
            }

            if (results.Success)
            {
                FluentMapper.Initialize(config =>
                {
                    config.AddMap(new ProfileMapping());
                    config.AddMap(new BaseEntityMapping());
                    config.AddMap(new EntertainmentTypeMapping());
                    config.ForDommel();
                });

                using (IDbConnection connection = new SQLiteConnection($"Data Source={fullPathToDB};Version=3;"))
                {
                    connection.Open();

                    Double versionCompare = Convert.ToDouble(versionToMigrateTo);
                    Double currentDBVersion = 00.00;
                    if (init)
                    {
                        connection.Execute("CREATE TABLE Versions (Id INTEGER PRIMARY KEY, Section VARCHAR(10) NOT NULL, Version VARCHAR(6) NOT NULL);");
                        connection.Execute($"INSERT INTO Versions (Section, Version) VALUES ('database', '{versionToMigrateTo}');");
                        currentDBVersion = versionCompare;
                        logging.Log("INIT:  Version table created.");
                    }
                    else
                    {
                        currentDBVersion = Convert.ToDouble(connection.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault());
                        logging.Log("MIGRATE:  Version table updated.");
                    }


                    if ((init && versionCompare >= .01) || (!init && (.01 > versionCompare && currentDBVersion < versionCompare)))
                    {
                        connection.Execute("CREATE TABLE Profile (ProfileId INTEGER PRIMARY KEY, UserName TEXT);");
                        if (!init)
                        {
                            connection.Execute("UPDATE Versions SET Version = '00.01' WHERE Section = 'database'");
                        }
                        logging.Log($"{(!init ? "MIGRATE" : "INIT")}:  v.01 Completed.");
                    }

                    if ((init && versionCompare >= .02) || (!init && (.02 > versionCompare && currentDBVersion < versionCompare)))
                    {
                        connection.Execute(@"
                            CREATE TABLE BaseEntity (
                                Id INTEGER PRIMARY KEY,
                                WhenAdded TEXT NOT NULL,
                                WhenUpdated TEXT NOT NULL,
                                ProfileId INTEGER NOT NULL,
                                FOREIGN KEY(ProfileId)
                                    REFERENCES Profile(ProfileId)
                                        ON DELETE CASCADE
                                        ON UPDATE NO ACTION
                            );"
                        );
                        if (!init)
                        {
                            connection.Execute("UPDATE Versions SET Version = '00.02' WHERE Section = 'database'");
                        }
                        logging.Log($"{(!init ? "MIGRATE" : "INIT")}:  v.02 Completed.");
                    }

                    if ((init && versionCompare >= .03) || (!init && (.03 >= versionCompare && currentDBVersion < versionCompare)))
                    {
                        connection.Execute(@"  
                            CREATE TABLE EntertainmentType (
                                Description TEXT NOT NULL,
                                Id INTEGER NOT NULL,
                                FOREIGN KEY(Id)
                                    REFERENCES BaseEntity(Id)
                                        ON DELETE CASCADE
                                        ON UPDATE NO ACTION
                            );"
                        );
                        EntertainmentType et = new EntertainmentType { Description = "Movie"};
                        int id = (int)connection.Insert(et);
                        if (!init)
                        {
                            connection.Execute("UPDATE Versions SET Version = '00.03' WHERE Section = 'database'");
                        }
                        logging.Log($"{(!init ? "MIGRATE" : "INIT")}:  v.03 Completed.");
                    }

                    if (results.Success)
                    {
                        Double postMigrationDBVersion =
                            Convert.ToDouble(
                                connection.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'")
                                .FirstOrDefault()
                            );
                        results.MigrationOccurred = postMigrationDBVersion > currentDBVersion;
                        results.Message = postMigrationDBVersion.ToString("00.00");
                        logging.Log($"{(!init ? "MIGRATE" : "INIT")}:  Database version set.");
                    }
                    connection.Close();

                }
            }

            return results;
        }
    }
}
