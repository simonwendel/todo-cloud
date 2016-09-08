/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016  Simon Wendel
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
    using System.Data.SqlClient;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal class SqlServerConnectionFactoryTests
    {
        [Test]
        public void Ctor_GivenNullConnectionStringResolver_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new SqlServerConnectionFactory(null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenConnectionStringResolver_GetsConnectionString()
        {
            var mockResolver = new Mock<IConnectionStringResolver>();
            mockResolver
                .SetupGet(r => r.ConnectionString)
                .Returns("whatever");

            var sut = new SqlServerConnectionFactory(mockResolver.Object);

            mockResolver.VerifyAll();
        }

        [Test]
        public void GetConnection_ReturnsSqlClient()
        {
            var resolver = new ConnectionStringResolver("TodoStorage");
            var sut = new SqlServerConnectionFactory(resolver);

            var connection = sut.GetConnection();

            Assert.That(connection, Is.Not.Null);
            Assert.That(connection, Is.TypeOf<SqlConnection>());
        }
    }
}
