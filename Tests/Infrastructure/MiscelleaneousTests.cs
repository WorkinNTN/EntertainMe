using System;

using NUnit.Framework;

using EntertainMe.Domain.Entities;
using EntertainMe.Infrastructure;

namespace EntertainMeTests.Infrastructure
{
    public class MiscelleaneousTests

    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EntertainMePathHasValue()
        {
            Assert.IsTrue(!string.IsNullOrEmpty(Constants.EntertainMePath));
        }
    }
}