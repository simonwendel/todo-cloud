﻿/*
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
    using System.Data.SqlClient;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal class ConnectionFactoryTests
    {
        [Test]
        public void Ctor_GivenNullConnectionStringResolver_ThrowsException()
        {
            Action constructing = () => new ConnectionFactory(null);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Ctor_GivenConnectionStringResolver_GetsConnectionString()
        {
            var connectionStringResolver = new Mock<IConnectionStringResolver>();
            connectionStringResolver.SetupGet(r => r.ConnectionString).Returns("whatever");

            var sut = new ConnectionFactory(connectionStringResolver.Object);

            connectionStringResolver.VerifyAll();
        }

        [Test]
        public void GetConnection_ReturnsSqlClient()
        {
            var resolver = new ConnectionStringResolver("TodoStorageTest");
            var sut = new ConnectionFactory(resolver);

            var connection = sut.GetConnection();
            
            connection.Should().BeOfType<SqlConnection>().And.NotBeNull();
        }
    }
}
