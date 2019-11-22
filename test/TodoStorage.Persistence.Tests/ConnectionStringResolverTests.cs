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
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal class ConnectionStringResolverTests
    {
        [Test]
        public void Ctor_GivenNullConnectionStringName_ThrowsException()
        {
            Action constructing = () => new ConnectionStringResolver(null);
            constructing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Ctor_GivenNonExistentConnectionStringName_ThrowsException()
        {
            Action constructing = () => new ConnectionStringResolver("Can'tFindThis");
            constructing.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void Ctor_GivenConnectionStringName_SetsConnectionString()
        {
            var sut = new ConnectionStringResolver("ShouldFindThis");
            sut.ConnectionString.Should().Be("w00t!");
        }

        [Test]
        public void Ctor_NullaryCtorCalled_SetsConnectionString()
        {
            var sut = new ConnectionStringResolver();
            sut.ConnectionString.Should().Be("DefaultConnectionStringLikeThis!");
        }
    }
}
