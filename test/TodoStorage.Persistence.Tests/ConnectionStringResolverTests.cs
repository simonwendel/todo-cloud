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
    using NUnit.Framework;

    [TestFixture]
    internal class ConnectionStringResolverTests
    {
        [Test]
        public void Ctor_GivenNullConnectionStringName_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new ConnectionStringResolver(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNonExistentConnectionStringName_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new ConnectionStringResolver("Can'tFindThis");

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenConnectionStringName_SetsConnectionString()
        {
            var sut = new ConnectionStringResolver("ShouldFindThis");

            Assert.That(sut.ConnectionString, Is.EqualTo("w00t!"));
        }

        [Test]
        public void Ctor_NullaryCtorCalled_SetsConnectionString()
        {
            var sut = new ConnectionStringResolver();

            Assert.That(
                sut.ConnectionString, 
                Is.EqualTo("DefaultConnectionStringLikeThis!"));
        }
    }
}
