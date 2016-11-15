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

namespace TodoStorage.Domain.Tests
{
    using System;
    using NUnit.Framework;
    using TodoStorage.Domain;

    [TestFixture]
    internal class CollectionKeyTests
    {
        [Test]
        public void Ctor_GivenEmptyGuid_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new CollectionKey(Guid.Empty);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenGuid_SetsIdentifier()
        {
            var identifier = Guid.NewGuid();
            var sut = new CollectionKey(identifier);

            Assert.That(sut.Identifier, Is.EqualTo(identifier));
        }

        [Test]
        public void Equals_GivenSameObject_ReturnsTrue()
        {
            var sut = new CollectionKey(Guid.NewGuid());

            Assert.That(sut.Equals(sut), Is.True);
        }

        [Test]
        public void Equals_GivenObjectWithSameProperties_ReturnsTrue()
        {
            var sut = new CollectionKey(Guid.NewGuid());
            var sameProperties = new CollectionKey(sut.Identifier);

            Assert.That(sut.Equals(sameProperties), Is.True);
        }

        [Test]
        public void Equals_GivenNull_ReturnsFalse()
        {
            var sut = new CollectionKey(Guid.NewGuid());

            Assert.That(sut.Equals(null), Is.False);
        }

        [Test]
        public void Equals_GivenObjectWithDifferingProperties_ReturnsFalse()
        {
            var sut = new CollectionKey(Guid.NewGuid());
            var differingProperties = new CollectionKey(Guid.NewGuid());

            Assert.That(sut.Equals(differingProperties), Is.False);
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

            Assert.That(sut.GetHashCode(), Is.EqualTo(hash));
        }
    }
}
