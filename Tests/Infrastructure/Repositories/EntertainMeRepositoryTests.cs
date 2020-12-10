using System;
using System.IO;
using System.Linq;

using Dapper;
using NUnit.Framework;

using EntertainMe.Domain.Entities;
using EntertainMe.Infrastructure;
using EntertainMe.Infrastructure.Repositories;

namespace EntertainMeTests.Infrastructure.Repositories
{
    public class EntertainMeRepositoryTests
    {
        public string testPath = Constants.EntertainMePath;
        public string testDBName = Constants.EntertainMeDB + "_repo_test.db";

        [SetUp]
        public void Setup()
        {
            if (!testPath.EndsWith(@"\"))
            {
                testPath = testPath + @"\";
            }
            if (File.Exists(testPath + testDBName))
            {
                File.Delete(testPath + testDBName);
            }
            if (Directory.Exists(testPath))
            {
                Directory.Delete(testPath);
            }
        }

        [Test]
        public void LoadRepository_CreateDatabaseFile_NonExist_latest_automigrate()
        {
            var repo = new EntertainMeRepository(testPath, testDBName, autoMigrate: true);
            Assert.True(Directory.Exists(testPath));
            Assert.True(File.Exists(testPath + testDBName));

            var version = repo.db.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault();
            Assert.True(!string.IsNullOrEmpty(version) && version == Constants.DBVersion);
        }

        [Test]
        public void LoadRepository_CreateDatabaseFile_NonExist_latest_noautomigrate()
        {
            var repo = new EntertainMeRepository(testPath, testDBName);
            Assert.True(Directory.Exists(testPath));
            Assert.True(File.Exists(testPath + testDBName));

            var version = repo.db.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault();
            Assert.True(!string.IsNullOrEmpty(version) && version == Constants.DBVersion);
        }

        [Test]
        public void LoadRepository_CreateDatabaseFile_NonExist_v00_01()
        {
            var repo = new EntertainMeRepository(testPath, testDBName, dbVersion: "00.01");
            Assert.True(Directory.Exists(testPath));
            Assert.True(File.Exists(testPath + testDBName));

            var version = repo.db.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault();
            Assert.True(!string.IsNullOrEmpty(version) && version == "00.01");

            var table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Profile';");
            var profileExists = table.FirstOrDefault();
            Assert.True((!string.IsNullOrEmpty(profileExists) && profileExists == "Profile"));

            table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'BaseEntity';");
            var baseEntityExists = table.FirstOrDefault();
            Assert.True(string.IsNullOrEmpty(baseEntityExists));

            table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'EntertainmentType';");
            var entertainmentTypeExists = table.FirstOrDefault();
            Assert.True(string.IsNullOrEmpty(entertainmentTypeExists));
        }

        [Test]
        public void LoadRepository_CreateDatabaseFile_NonExist_v00_02()
        {
            var repo = new EntertainMeRepository(testPath, testDBName, dbVersion: "00.02");
            Assert.True(Directory.Exists(testPath));
            Assert.True(File.Exists(testPath + testDBName));

            var version = repo.db.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault();
            Assert.True(!string.IsNullOrEmpty(version) && version == "00.02");

            var table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Profile';");
            var profileExists = table.FirstOrDefault();
            Assert.True((!string.IsNullOrEmpty(profileExists) && profileExists == "Profile"));

            table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'BaseEntity';");
            var baseEntityExists = table.FirstOrDefault();
            Assert.True((!string.IsNullOrEmpty(baseEntityExists) && baseEntityExists == "BaseEntity"));

            table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'EntertainmentType';");
            var entertainmentTypeExists = table.FirstOrDefault();
            Assert.True(string.IsNullOrEmpty(entertainmentTypeExists));
        }

        [Test]
        public void LoadRepository_CreateDatabaseFile_NonExist_v00_03()
        {
            var repo = new EntertainMeRepository(testPath, testDBName, dbVersion: "00.03");
            Assert.True(Directory.Exists(testPath));
            Assert.True(File.Exists(testPath + testDBName));

            var version = repo.db.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault();
            Assert.True(!string.IsNullOrEmpty(version) && version == "00.03");

            var table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'Profile';");
            var profileExists = table.FirstOrDefault();
            Assert.True((!string.IsNullOrEmpty(profileExists) && profileExists == "Profile"));

            table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'BaseEntity';");
            var baseEntityExists = table.FirstOrDefault();
            Assert.True((!string.IsNullOrEmpty(baseEntityExists) && baseEntityExists == "BaseEntity"));

            table = repo.db.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'EntertainmentType';");
            var entertainmentTypeExists = table.FirstOrDefault();
            Assert.True((!string.IsNullOrEmpty(entertainmentTypeExists) && entertainmentTypeExists == "EntertainmentType"));
        }

        [Test]
        public void LoadRepository_CreateDatabaseFile_NonExist_A_Version_UpgradeTo_latest()
        {
            string versionToInit = "00.02";

            var repo = new EntertainMeRepository(testPath, testDBName, dbVersion: versionToInit);
            Assert.True(Directory.Exists(testPath));
            Assert.True(File.Exists(testPath + testDBName));

            var version = repo.db.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault();
            Assert.True(!string.IsNullOrEmpty(version) && version == versionToInit);
            Assert.False(repo.MigrationOccurred);

            repo.MigrateDatabase();
            version = repo.db.Query<string>(@"SELECT Version FROM Versions WHERE Section = 'database'").FirstOrDefault();
            Assert.True(!string.IsNullOrEmpty(version) && version == Constants.DBVersion);
            Assert.True(repo.MigrationOccurred);
        }
    }
}