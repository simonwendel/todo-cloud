/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016-2017  Simon Wendel
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace TodoStorage.Persistence.Tests
{
    using System;
    using NUnit.Framework;
    using TodoStorage.Persistence.Tests.Seed;

    [TestFixture]
    internal class AuthenticationRepositoryTests
    {
        private AuthenticationRepository sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var seeder = new DatabaseSeeder();
            seeder.InjectSeed();
        }

        [SetUp]
        public void Setup()
        {
            var resolver = new ConnectionStringResolver("TodoStorageTest");
            var connectionFactory = new ConnectionFactory(resolver);

            sut = new AuthenticationRepository(connectionFactory);
        }

        [Test]
        public void Ctor_GivenNullConnectionFactory_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AuthenticationRepository(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void GetSecret_GivenNonExistentAppId_ReturnsNull()
        {
            var secretBytes = sut.GetSecret(Guid.NewGuid());

            Assert.That(secretBytes, Is.Null);
        }

        [Test]
        public void GetSecret_GivenAppId_ReturnsSecret()
        {
            var secretBytes = sut.GetSecret(Seed.Data.OtherAuth.AppId);

            Assert.That(secretBytes, Is.EquivalentTo(Seed.Data.OtherAuth.Secret));
        }
    }
}
