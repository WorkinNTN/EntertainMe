using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Dapper;
using System.Data.SQLite;

using EntertainMe.Domain.ValueObjects;

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

            if (results.Success)
            {
                try
                {
                    if (File.Exists(fullPathToDB) && init)
                    {
                        File.Delete(fullPathToDB);
                    }
                    if (!File.Exists(fullPathToDB))
                    {
                        Directory.CreateDirectory(new FileInfo(fullPathToDB).Directory.FullName);
                        SQLiteConnection.CreateFile(fullPathToDB);
                        init = true;
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
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={fullPathToDB};Version=3;"))
                {
                    connection.Open();

                    Double versionCompare = Convert.ToDouble(versionToMigrateTo);
                    Double currentDBVersion = 00.00;
                    if (init)
                    {
                        connection.Execute("CREATE TABLE Versions (Id INTEGER PRIMARY KEY, Section VARCHAR(10) NOT NULL, Version VARCHAR(6) NOT NULL);");
                        connection.Execute($"INSERT INTO Versions (Section, Version) VALUES ('database', '{versionToMigrateTo}');");
                        currentDBVersion = versionCompare;
                    }
                    else
                    {
                        currentDBVersion = Convert.ToDouble(connection.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault());
                    }


                    if ((init && versionCompare >= .01) || (!init && (.01 > versionCompare && currentDBVersion < versionCompare)))
                    {
                        connection.Execute("CREATE TABLE Profile (ProfileId INTEGER PRIMARY KEY, UserName TEXT);");
                        if (!init)
                        {
                            connection.Execute("UPDATE Versions SET Version = '00.01' WHERE Section = 'database'");
                        }
                    }

                    if ((init && versionCompare >= .02) || (!init && (.02 > versionCompare && currentDBVersion < versionCompare)))
                    {
                        connection.Execute(@"
                            CREATE TABLE BaseEntity (
                                Id INTEGER PRIMARY KEY,
                                Added TEXT NOT NULL,
                                Updated TEXT NOT NULL,
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
                        if (!init)
                        {
                            connection.Execute("UPDATE Versions SET Version = '00.03' WHERE Section = 'database'");
                        }
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
                    }
                    connection.Close();

                }
            }

            return results;
        }
    }
}
