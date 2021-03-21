using System;
using System.IO;
using System.Linq;

using LiteDB;
using NUnit.Framework;

using EntertainMe.Domain.Entities;
using EntertainMe.Infrastructure;
using EntertainMe.Infrastructure.Repositories;

namespace EntertainMeTests.Infrastructure.Repositories
{
    [TestFixture]
    public class EntertainMeRepositoryTests
    {
        public string testPath = EMConstants.EntertainMePath;
        public string testDBName = EMConstants.EntertainMeDB + "_repo_test.db";
        public EntertainMeRepository testRepo;

        [SetUp]
        public void Setup()
        {
            if (!testPath.EndsWith(@"\"))
            {
                testPath = testPath + @"\";
            }
            testRepo = new EntertainMeRepository();
        }

        [TearDown]
        public void TearDown()
        {
            testRepo = null;
        }

        [Test]
        public void CreateNonExistingDB()
        { 
            var repo = new EntertainMeRepository(testPath, testDBName);
            Assert.True(File.Exists(testPath + testDBName));
            var profile = repo.GetEMProfileByName("New User");
            Assert.AreEqual("New User", profile.UserName);
            repo.Dispose();
            if (File.Exists(testPath + testDBName))
            {
                File.Delete(testPath + testDBName);
            }
            if (Directory.Exists(testPath))
            {
                Directory.Delete(testPath, true);
            }
        }

        [Test]
        public void DefaultDBInitialized()
        {
            var profile = testRepo.GetEMProfileByName("New User");
            Assert.AreEqual("New User", profile.UserName);
        }

        [Test]
        public void SaveNewProfile()
        {
            _ = testRepo.SaveEMProfile(new EMProfile { UserName = "Test Guy" });
            var profile = testRepo.GetEMProfileByName("Test Guy");
            Assert.NotNull(profile);
            Assert.AreEqual("Test Guy", profile.UserName);
        }

        [Test]
        public void SaveExistingProfile()
        {
            testRepo.SaveEMProfile(new EMProfile { UserName = "Test Guy" });
            var profile = testRepo.GetEMProfileByName("Test Guy");
            Assert.NotNull(profile);
            Assert.AreEqual("Test Guy", profile.UserName);

            profile.UserName = "New Guy";
            _ = testRepo.SaveEMProfile(profile);
            var updatedProfile = testRepo.GetEMProfileByName("New Guy");
            Assert.AreEqual("New Guy", updatedProfile.UserName);
            Assert.AreEqual(profile.Id, updatedProfile.Id);
            
        }

        [Test]
        public void GetAllEntertainmentTypes()
        {
            var results = testRepo.GetEMTypes();

            Assert.GreaterOrEqual(results.Count(), 3);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "movie")) == 1);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "music")) == 1);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "book")) == 1);
        }

        [Test]
        public void GetEntertainmentType()
        {
            var result = testRepo.GetEMTypeByName("Music");
            Assert.NotNull(result);
            Assert.AreEqual("Music", result.Description);
        }

        [Test]
        public void SaveNewEntertainmentType()
        {
            _ = testRepo.SaveEMType(new EntertainMe.Domain.Entities.EMType { Description = "Something New" });
            var et = testRepo.GetEMTypeByName("Something New");
            Assert.NotNull(et);
            Assert.AreEqual("Something New", et.Description);
        }

        [Test]
        public void SaveExistingEntertainmentType()
        {
            _ = testRepo.SaveEMType(new EntertainMe.Domain.Entities.EMType { Description = "Something New" });
            var et = testRepo.GetEMTypeByName("Something New");
            Assert.NotNull(et);
            Assert.AreEqual("Something New", et.Description);

            et.Description = "Something Changed";
            _ = testRepo.SaveEMType(et);
            var updatedET = testRepo.GetEMTypeByName("Something Changed");
            Assert.AreEqual("Something Changed", updatedET.Description);
            Assert.AreEqual(et.Id, updatedET.Id);

        }

        [Test]
        public void GetAllProviders()
        {
            var results = testRepo.GetEMProviders();

            Assert.GreaterOrEqual(results.Count(), 6);
            Assert.IsTrue(results.Count(p => (p.Description.ToLower() == "vudu")) == 1);
            Assert.IsTrue(results.Count(p => (p.Description.ToLower() == "amazon")) == 1);
            Assert.IsTrue(results.Count(p => (p.Description.ToLower() == "google")) == 1);
        }

        [Test]
        public void GetEntertainmentProvider()
        {
            var result = testRepo.GetEMProviderByName("Amazon");
            Assert.NotNull(result);
            Assert.AreEqual("Amazon", result.Description);
        }

        [Test]
        public void SaveNewProviderType()
        {
            _ = testRepo.SaveEMProvider(new EMProvider { Description = "Something New" });
            var p = testRepo.GetEMProviderByName("Something New");
            Assert.NotNull(p);
            Assert.AreEqual("Something New", p.Description);
        }

        [Test]
        public void SaveExistingProvider()
        {
            _ = testRepo.SaveEMProvider(new EMProvider { Description = "Something New" });
            var p = testRepo.GetEMProviderByName("Something New");
            Assert.NotNull(p);
            Assert.AreEqual("Something New", p.Description);

            p.Description = "Something Changed";
            _ = testRepo.SaveEMProvider(p);
            var updatedP = testRepo.GetEMProviderByName("Something Changed");
            Assert.AreEqual("Something Changed", updatedP.Description);
            Assert.AreEqual(p.Id, updatedP.Id);

        }

        [Test]
        public void GetAllEntertainmentMediums()
        {
            var results = testRepo.GetEMMediums();

            Assert.GreaterOrEqual(results.Count(), 6);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "cd")) == 1);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "dvd")) == 1);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "cassette")) == 1);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "hard cover")) == 1);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "soft cover")) == 1);
            Assert.IsTrue(results.Count(et => (et.Description.ToLower() == "digital")) == 1);
        }

        [Test]
        public void GetEntertainmentMedium()
        {
            var result = testRepo.GetEMMediumByName("DVD");
            Assert.NotNull(result);
            Assert.AreEqual("DVD", result.Description);
        }

        [Test]
        public void SaveNewEntertainmentMedium()
        {
            _ = testRepo.SaveEMMedium(new EMMedium { Description = "Something New" });
            var em = testRepo.GetEMMediumByName("Something New");
            Assert.NotNull(em);
            Assert.AreEqual("Something New", em.Description);
        }

        [Test]
        public void SaveExistingEntertainmentMedium()
        {
            _ = testRepo.SaveEMMedium(new EMMedium { Description = "Something New" });
            var em = testRepo.GetEMMediumByName("Something New");
            Assert.NotNull(em);
            Assert.AreEqual("Something New", em.Description);

            em.Description = "Something Changed";
            _ = testRepo.SaveEMMedium(em);
            var updatedEM = testRepo.GetEMMediumByName("Something Changed");
            Assert.AreEqual("Something Changed", updatedEM.Description);
            Assert.AreEqual(em.Id, updatedEM.Id);

        }

        [Test]
        public void GetValidMediumsForBooksByDescription()
        {
            var mediums = testRepo.GetValidMediumsForType("Book");

            Assert.IsNotNull(mediums);
            Assert.AreEqual(5, mediums.Count());
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "cd") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "cassette") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "hard cover") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "soft cover") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "digital") == 1);
        }

        [Test]
        public void GetValidMediumsForMusicsById()
        {
            var type = testRepo.GetEMTypeByName("music");
            var mediums = testRepo.GetValidMediumsForType(type.Id);

            Assert.IsNotNull(mediums);
            Assert.AreEqual(3, mediums.Count());
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "cd") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "cassette") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "digital") == 1);
        }

        [Test]
        public void GetValidMediumsForMoviesByObject()
        {
            var type = testRepo.GetEMTypeByName("movie");
            var mediums = testRepo.GetValidMediumsForType(type);

            Assert.IsNotNull(mediums);
            Assert.AreEqual(3, mediums.Count());
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "dvd") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "video cassette") == 1);
            Assert.IsTrue(mediums.Count(mediums => mediums.EMMedium.Description.ToLower() == "digital") == 1);
        }

        [Test]
        public void GetValidTypesForCDsByDescription()
        {
            var types = testRepo.GetValidTypesForMedium("cd");

            Assert.IsNotNull(types);
            Assert.AreEqual(2, types.Count());
            Assert.IsTrue(types.Count(types => types.EMType.Description.ToLower() == "music") == 1);
            Assert.IsTrue(types.Count(types => types.EMType.Description.ToLower() == "book") == 1);
        }

        [Test]
        public void GetValidTypesForDigitalById()
        {
            var medium = testRepo.GetEMMediumByName("digital");
            var types = testRepo.GetValidTypesForMedium(medium.Id);

            Assert.IsNotNull(types);
            Assert.AreEqual(3, types.Count());
            Assert.IsTrue(types.Count(types => types.EMType.Description.ToLower() == "music") == 1);
            Assert.IsTrue(types.Count(types => types.EMType.Description.ToLower() == "book") == 1);
            Assert.IsTrue(types.Count(types => types.EMType.Description.ToLower() == "movie") == 1);
        }

        [Test]
        public void GetValidTypesForDVDByObject()
        {
            var medium = testRepo.GetEMMediumByName("dvd");
            var types = testRepo.GetValidTypesForMedium(medium);

            Assert.IsNotNull(types);
            Assert.AreEqual(1, types.Count());
            Assert.IsTrue(types.Count(types => types.EMType.Description.ToLower() == "movie") == 1);
        }

        [Test]
        public void SaveAsset()
        {
            var profile = testRepo.GetEMProfileByName("New User");
            var asset1 = new EMAsset() { EMProfile = profile, Title = "New Asset 1", Description = "This is a unit test asset" };
            var asset2 = new EMAsset() { EMProfile = profile, Title = "New Asset 2", Description = "This is a unit test asset" };
            asset1 = testRepo.SaveEMAsset(asset1);
            asset2 = testRepo.SaveEMAsset(asset2);

            var assets = testRepo.GetEMAssets(profile);

            Assert.IsNotNull(assets);
            Assert.AreEqual(2, assets.Count());
            Assert.AreEqual("New Asset 1", assets.Find((x => x.Id == asset1.Id)).Title);
            Assert.AreEqual("New Asset 2", assets.Find((x => x.Id == asset2.Id)).Title);
        }
    }
}
