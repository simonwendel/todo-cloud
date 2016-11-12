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

namespace TodoStorage.Security.Tests
{
    using NUnit.Framework;
    using Security;

    [TestFixture]
    internal class CollectionSecurityKeyTests
    {
        [Test]
        public void Ctor_GivenEmptyGuid_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new CollectionSecurityKey(System.Guid.Empty, new byte[100]);

            Assert.That(constructorCall, Throws.ArgumentException);
        }

        [Test]
        public void Ctor_GivenNullSecret_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new CollectionSecurityKey(System.Guid.NewGuid(), null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenZeroLengthSecret_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new CollectionSecurityKey(System.Guid.NewGuid(), new byte[0]);

            Assert.That(constructorCall, Throws.ArgumentException);
        }
    }
}
