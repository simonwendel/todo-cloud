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

namespace TodoStorage.Core.Tests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using TodoStorage.Core;

    [TestFixture]
    internal class CollectionKeyTests
    {
        [Test]
        public void Ctor_GivenEmptyGuid_ThrowsException()
        {
            Action constructing = () => new CollectionKey(Guid.Empty);
            constructing.Should().ThrowExactly<ArgumentException>();
        }

        [Test]
        public void Ctor_GivenGuid_SetsIdentifier()
        {
            var identifier = Guid.NewGuid();
            var sut = new CollectionKey(identifier);
            sut.Identifier.Should().Be(identifier);
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = new CollectionKey(Guid.NewGuid());
            sut.Equals(sut).Should().BeTrue();
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            var sut = new CollectionKey(Guid.NewGuid());
            var sameProperties = new CollectionKey(sut.Identifier);
            sut.Equals(sameProperties).Should().BeTrue();
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = new CollectionKey(Guid.NewGuid());
            sut.Equals(null).Should().BeFalse();
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var sut = new CollectionKey(Guid.NewGuid());
            var differingProperties = new CollectionKey(Guid.NewGuid());
            sut.Equals(differingProperties).Should().BeFalse();
        }

        [Test]
        public void GetHashCode_ReturnsHashByProperties()
        {
            var sut = new CollectionKey(Guid.NewGuid());

            var start = 17;
            var multiplier = 486187739;

            int hash;
            unchecked
            {
                hash = start;
                hash = (hash * multiplier) + sut.Identifier.GetHashCode();
            }

            sut.GetHashCode().Should().Be(hash);
        }
    }
}
